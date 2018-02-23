using System;
using System.ComponentModel.DataAnnotations;
namespace belt1.Models
{
    public class RegisterActivityModel
    {
        [Required]
        [MinLength(2)]
        public string Title { get; set; }

        [Required]
        [NonPast(ErrorMessage="You must put a valid date in the future.")]
        public DateTime Date { get; set; }
 
        [Required]
        public DateTime Time { get; set; }
        
        [Required]
        public int Duration { get; set; }
        
        [Required]
        [Display(Name=" ")]
        public string Metric { get; set; }
 
        [Required]
        [MinLength(10, ErrorMessage="Description must be at least 10 characters")]
        public string Description { get; set; }
    }
}