using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using belt1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace belt1.Controllers
{
    public class UsersController : Controller
    {
        private MyContext _context;
    
        public UsersController(MyContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View("Register");
        }

        [HttpGet]
        [Route("Register")]
        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        [Route("NewUser")]
        public IActionResult NewUser(RegisterUserModel model)
        {
            List<User> email = _context.users.Where(user => user.Email == model.Email).ToList();
            if(email.Count > 0) {
                ModelState.AddModelError("Email", "that email is already registered");
            }
            if(ModelState.IsValid)
            {
                TempData["FirstName"] = model.FirstName;
                TempData["LastName"] = model.LastName;
                TempData["Email"] = model.Email;
                TempData["Password"] = model.Password;
                return RedirectToAction("RegisterUser");
            }
            return View("Register", model);
        }

        [HttpGet]
        [Route("RegisterUser")]
        public IActionResult RegisterUser()
        {
            //hash pw here
            User NewUser = new User
            {
                FirstName = TempData["FirstName"] as string,
                LastName = TempData["LastName"] as string,
                Email = TempData["Email"] as string,
                Password = TempData["Password"] as string,
            };
            _context.users.Add(NewUser);
            _context.SaveChanges();
            ViewBag.SuccessMsg = "You've successfully created your account. Now log on in.";
            return View("Register");
        }

        [HttpGet]
        [Route("Login")]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        [Route("LoginUser")]
        public IActionResult LoginUser(LoginUserModel model)
        {
            List<User> email = _context.users.Where(user => user.Email == model.LoginEmail).ToList();
            if(email.Count == 0) {
                ModelState.AddModelError("LoginEmail", "that email doesn't exist");
            }
            else
            {
                if(email[0].Password.ToString() != model.LoginPassword)
                {
                    ModelState.AddModelError("LoginPassword", "password is incorrect.");
                }
            }
            if(ModelState.IsValid)
            {
                HttpContext.Session.SetInt32("CurrUserId", (int)email[0].Id);
                return RedirectToAction("Success");
            }

            return View("Login", model);
        }

        [HttpGet]
        [Route("Logout")]
        public IActionResult Logout(LoginUserModel model)
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index");
        }
        
        [HttpGet]
        [Route("Success")]
        public IActionResult Success()
        {
            return RedirectToAction("ActivityCenter", "Belt");
        }
        
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
