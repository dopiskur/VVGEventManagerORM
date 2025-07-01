using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "User name is required")]
        [DisplayName("Username")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DisplayName("Password")]
        [StringLength(256, MinimumLength = 8, ErrorMessage = "Password should be at least 8 characters long")]
        public string? Password { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
