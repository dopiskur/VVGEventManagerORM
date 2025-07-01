using eventLib.Security;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace eventLib.Models
{
    public class User
    {
        [HiddenInput]
        public int? IDUser { get; set; }
        [DisplayName("Username")]
        [Required(ErrorMessage = "User name is required")]
        public string? Username { get; set; } = null!;
        [DisplayName("Password")]
        public string? PwdHash { get; set; } = null!;
        public string? PwdSalt { get; set; } = null!;
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

        public int? UserRoleId { get; set; }
        public string? RoleName { get; set;}

        // Navigation properties for Entity Framework
        public virtual UserRole? UserRole { get; set; }
        public virtual ICollection<EventRegistration> EventRegistrations { get; set; } = new List<EventRegistration>();
    }
}
