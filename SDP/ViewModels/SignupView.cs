using System.ComponentModel.DataAnnotations;

namespace SDP.ViewModels
{
    public class SignupView
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirm password do not match")]
        [Display(Name = "Re - type Password")]
        public string ReenterPassword { get; set; }
        [Display(Name = "Recive promotion email")]
        public bool opIn { get; set; }
    }
}
