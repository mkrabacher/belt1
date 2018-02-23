using System;
using System.ComponentModel.DataAnnotations;

namespace belt1.Models
{
    public class NonPastAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value != null && (DateTime)value >= DateTime.Now;
        }
    }
}