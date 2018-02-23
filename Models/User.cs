using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace belt1.Models
{
    public class User : Base
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        
        [InverseProperty("Creator")]
        public List<Activity> ActivitiesCreated { get; set; }
        
        [InverseProperty("User")]
        public List<UserActivity> Activities { get; set; }
        
        public User()
        {
            ActivitiesCreated = new List<Activity>();
            Activities = new List<UserActivity>();
        }
    }
}