using System.ComponentModel.DataAnnotations;

namespace SDP.ViewModels
{
    public class SignupView
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Please ensure your passwords match")]
        [Display(Name = "Re-type Password")]
        public string ReenterPassword { get; set; }

        [Display(Name = "Recive promotion email")]
        public bool opIn { get; set; }
    }
}
