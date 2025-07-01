using eventLib.Security;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace eventLib.Models
{
    public class UserRegistration
    {

        [Required(ErrorMessage = "User name is required")]
        [DisplayName("Username")]
        public string? Username { get; set; } = null!;
        [Required]
        [StringLength(256, MinimumLength = 8, ErrorMessage = "Password should be at least 8 characters long")]
        [DisplayName("Password")]
        public string? Password { get; set; } = null!;
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name should be between 2 and 50 characters long")]
        [DisplayName("First name")]
        public string? FirstName { get; set; } = null!;
        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name should be between 2 and 50 characters long")]
        [DisplayName("Last name")]
        public string? LastName { get; set; } = null!;
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Provide a correct e-mail address")]
        [DisplayName("Email")]
        public string? Email { get; set; } = null!;
        [Required(ErrorMessage = "Phone is required")]
        [Phone(ErrorMessage = "Provide a correct phone number")]
        [DisplayName("Phone")]
        public string? Phone { get; set; }

    }


}
