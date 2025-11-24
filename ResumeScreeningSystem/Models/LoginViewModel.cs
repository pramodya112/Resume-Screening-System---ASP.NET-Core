using System.ComponentModel.DataAnnotations;

namespace ResumeScreeningSystem.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username/Email is required")]
        [Display(Name = "Username/Email")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}