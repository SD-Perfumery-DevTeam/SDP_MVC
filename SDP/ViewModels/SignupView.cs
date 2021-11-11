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
        [Compare("Password", ErrorMessage = "Please ensure your passwords match")]
        [Display(Name = "Re-type Password")]
        public string ReenterPassword { get; set; }
        [Display(Name = "Receive promotional emails")]
        public bool opIn { get; set; }
    }
}
