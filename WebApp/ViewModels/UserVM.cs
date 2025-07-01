using eventLib.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class UserVM
    {
        [HiddenInput]
        public int? IDUser { get; set; }

        [Required(ErrorMessage = "User name is required")]
        [DisplayName("Username")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DisplayName("Password")]
        [StringLength(256, MinimumLength = 8, ErrorMessage = "Password should be at least 8 characters long")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [DisplayName("First name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name should be between 2 and 50 characters long")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [DisplayName("Last name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name should be between 2 and 50 characters long")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DisplayName("Email")]
        [EmailAddress(ErrorMessage = "Provide a correct e-mail address")]
        public string? Email { get; set; }

        [DisplayName("Phone")]
        [Required(ErrorMessage = "Phone is required")]
        [Phone(ErrorMessage = "Provide a correct phone number")]
        public string? Phone { get; set; }



        public UserEdit? UserEdit { get; set; } = new UserEdit();

        public IEnumerable<User>? Users { get; set; } = new List<User>();

        public IEnumerable<UserRole>? UserRoles { get; set; }
    }


    public class UserEdit
    {
        [HiddenInput]
        public int? IDUser { get; set; }
        [Required(ErrorMessage = "User name is required")]
        [DisplayName("Username")]
        public string? Username { get; set; } = null!;
        [StringLength(256, MinimumLength = 8, ErrorMessage = "Password should be at least 8 characters long")]
        [DisplayName("Password")]
        public string? Password { get; set; } = null!;
        [Required(ErrorMessage = "First name is required")]
        [DisplayName("First name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name should be between 2 and 50 characters long")]
        public string? FirstName { get; set; } = null!;
        [Required(ErrorMessage = "Last name is required")]
        [DisplayName("Last name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name should be between 2 and 50 characters long")]
        public string? LastName { get; set; } = null!;
        [Required(ErrorMessage = "Email is required")]
        [DisplayName("Email")]
        [EmailAddress(ErrorMessage = "Provide a correct e-mail address")]
        public string? Email { get; set; } = null!;
        [Required(ErrorMessage = "Phone is required")]
        [DisplayName("Phone")]
        [Phone(ErrorMessage = "Provide a correct phone number")]
        public string? Phone { get; set; }
        [Required]
        public int? UserRoleId { get; set; }
        [DisplayName("Role name")]
        public string? RoleName { get; set; }

    }


}
