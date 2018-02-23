using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using belt1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace belt1.Controllers
{
    public class BeltController : Controller
    {
        private MyContext _context;
    
        public BeltController(MyContext context)
        {
            _context = context;
        }
        
        public IActionResult ActivityCenter()
        {
            if(HttpContext.Session.GetInt32("CurrUserId") == null)
            {
                return RedirectToAction("Index", "Users");
            }
            List<Activity> activities = _context.Activities.Where(act => act.Date < DateTime.Now).ToList();
            foreach(Activity act in activities)
            {
                _context.Remove(act);
                _context.SaveChanges();
            }

            ViewBag.LoggedUser = _context.Users.SingleOrDefault(use => use.Id == HttpContext.Session.GetInt32("CurrUserId"));
            ViewBag.Activities = _context.Activities.Include("Creator").Include("Players").Where(act => act.Id > 0).ToList();
            return View("ActivityCenter");
        }

        [HttpGet]
        [Route("NewActivity")]
        public IActionResult NewActivity()
        {
            if(HttpContext.Session.GetInt32("CurrUserId") == null)
            {
                return RedirectToAction("Index", "Users");
            }
            ViewBag.LoggedUser = _context.Users.SingleOrDefault(use => use.Id == HttpContext.Session.GetInt32("CurrUserId"));
            return View("NewActivity");
        }

        [HttpPost]
        [Route("CreateActivity")]
        public IActionResult CreateActivity(RegisterActivityModel model)
        {
            if(ModelState.IsValid)
            {
                TempData["Title"] = model.Title;
                TempData["Date"] = String.Format("{0:MM/dd/yyyy}",model.Date);
                TempData["Time"] = String.Format("{0:t}", model.Time);
                TempData["Duration"] = model.Duration.ToString() + " " + model.Metric;
                TempData["Description"] = model.Description;
                return RedirectToAction("RegisterActivity");
            }
            
            ViewBag.LoggedUser = _context.Users.SingleOrDefault(use => use.Id == HttpContext.Session.GetInt32("CurrUserId"));
            return View("NewActivity", model);
        }

        [HttpGet]
        [Route("RegisterActivity")]
        public IActionResult RegisterActivity()
        {
            if(HttpContext.Session.GetInt32("CurrUserId") == null)
            {
                return RedirectToAction("Index", "Users");
            }
            //hash pw here
            System.Console.WriteLine(TempData["Date"]);
            System.Console.WriteLine(TempData["Time"]);
            Activity NewActivity = new Activity
            {
                Title = TempData["Title"] as string,
                Date = Convert.ToDateTime(TempData["Date"]),
                Time = Convert.ToDateTime(TempData["Time"]),
                Duration = TempData["Duration"] as string,
                Description = TempData["Description"] as string,
                CreatorId = (int)HttpContext.Session.GetInt32("CurrUserId"),
            };
            _context.Activities.Add(NewActivity);
            _context.SaveChanges();
            Activity act = _context.Activities.Last();
            return RedirectToAction("Activity", new {act.Id});
        }

        [HttpGet]
        [Route("Activity/{id}")]
        public IActionResult Activity(int id)
        {
            if(HttpContext.Session.GetInt32("CurrUserId") == null)
            {
                return RedirectToAction("Index", "Users");
            }
            Activity activity = _context.Activities.Include(act => act.Creator).Include(act => act.Players).ThenInclude(pla => pla.User).SingleOrDefault(act => act.Id == id);
            ViewBag.LoggedUser = _context.Users.SingleOrDefault(use => use.Id == HttpContext.Session.GetInt32("CurrUserId"));
            return View("Activity", activity);
        }

        [HttpGet]
        [Route("JoinActivity/{id}")]
        public IActionResult JoinActivity(int id)
        {
            if(HttpContext.Session.GetInt32("CurrUserId") == null)
            {
                return RedirectToAction("Index", "Users");
            }
            Activity activity = _context.Activities.Include("Creator").Include("Players").SingleOrDefault(act => act.Id == id);
            User user = _context.Users.SingleOrDefault(use => use.Id == (int)HttpContext.Session.GetInt32("CurrUserId"));
            List<UserActivity> ua = _context.UserActivities.Where(use => use.UserId == user.Id && use.ActivityId == id).ToList();
            if(ua.Count == 0)
            {
                UserActivity NewPlayer = new UserActivity()
                {
                    UserId = user.Id,
                    ActivityId = activity.Id,
                };
                _context.Add(NewPlayer);
                _context.SaveChanges();
                return RedirectToAction("ActivityCenter");   
            }
            ViewBag.LoggedUser = _context.Users.SingleOrDefault(use => use.Id == HttpContext.Session.GetInt32("CurrUserId"));
            return View("Activity", new {id});
        }

        [HttpGet]
        [Route("LeaveActivity/{id}")]
        public IActionResult LeaveActivity(int id)
        {
            if(HttpContext.Session.GetInt32("CurrUserId") == null)
            {
                return RedirectToAction("Index", "Users");
            }
            Activity activity = _context.Activities.Include("Creator").Include("Players").SingleOrDefault(act => act.Id == id);
            User user = _context.Users.SingleOrDefault(use => use.Id == (int)HttpContext.Session.GetInt32("CurrUserId"));
            UserActivity ua = _context.UserActivities.SingleOrDefault(uac => uac.UserId == user.Id && uac.ActivityId == activity.Id);
            _context.Remove(ua);
            _context.SaveChanges();
            return RedirectToAction("ActivityCenter");
        }

        [HttpGet]
        [Route("DeleteActivity/{id}")]
        public IActionResult DeleteActivity(int id)
        {
            if(HttpContext.Session.GetInt32("CurrUserId") == null)
            {
                return RedirectToAction("Index", "Users");
            }
            Activity activity = _context.Activities.Include("Creator").Include("Players").SingleOrDefault(act => act.Id == id);
            User user = _context.Users.SingleOrDefault(use => use.Id == (int)HttpContext.Session.GetInt32("CurrUserId"));
            List<UserActivity> ua = _context.UserActivities.Where(uac => uac.ActivityId == activity.Id).ToList();
            foreach(UserActivity act in ua)
            {
                _context.Remove(act);
            }
            _context.Remove(activity);
            _context.SaveChanges();
            return RedirectToAction("ActivityCenter");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
