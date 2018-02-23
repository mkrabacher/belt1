using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace belt1.Models
{
    public class Activity : Base
    {
        public string Title { get; set; }
        public DateTime Time { get; set; }
        public DateTime Date { get; set; }
        public string Duration { get; set; }
        public string Description { get; set; }

        [ForeignKey("User")]
        public int CreatorId { get; set; }
        public User Creator { get; set; }
        
        [InverseProperty("Activity")]
        public List<UserActivity> Players { get; set; }
        
        public Activity()
        {
            Players = new List<UserActivity>();
        }
    }
}