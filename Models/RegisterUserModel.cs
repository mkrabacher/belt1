using System.ComponentModel.DataAnnotations;
namespace belt1.Models
{
    public class RegisterUserModel
    {
        [Required]
        [Display(Name="First Name")]
        [MinLength(2)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage="First Name can only be English Alphabet Characters")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name="Last Name")]
        [MinLength(2)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage="Last Name can only be English Alphabet Characters")]
        public string LastName { get; set; }
 
        [Required]
        [Display(Name="Email")]
        [EmailAddress]
        public string Email { get; set; }
 
        [Required]
        [MinLength(8, ErrorMessage="Password must be at least 8 characters")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-zA-Z]).{8,30}$", ErrorMessage = "Password must have at least one lowercase and uppercase letter and at least one number as well as be at least 8 chars long.")]
        [Compare("PasswordConfirmation", ErrorMessage = "Password and Confirm Password must match.")]
        public string Password { get; set; }
 
        [Required]
        [Display(Name="Confirm Password")]
        [MinLength(8, ErrorMessage="Password must be at least 8 characters")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-zA-Z]).{8,30}$", ErrorMessage = "Password must have at least one lowercase and uppercase letter and at least one number as well as be at least 8 chars long.")]
        public string PasswordConfirmation { get; set; }
    }

    public class LoginUserModel
    { 
        [Required]
        [Display(Name="Email")]
        [EmailAddress]
        public string LoginEmail { get; set; }
 
        [Required]
        [Display(Name="Password")]
        [MinLength(8, ErrorMessage="Password must be at least 8 characters")]
        [DataType(DataType.Password)]
        public string LoginPassword { get; set; }
    }
}
