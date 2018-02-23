using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace belt1.Models
{
    public class UserActivity : Base
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int ActivityId { get; set; }
        public Activity Activity { get; set; }
    }
}