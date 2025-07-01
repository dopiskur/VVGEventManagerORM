using Azure.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eventLib.Models
{
    public class UserChangePassword
    {
        [Required]
        [DisplayName("Username")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Old password name is required")]
        [DisplayName("Old password")]
        public string OldPassword { get; set; }
        [Required]
        [StringLength(256, MinimumLength = 8, ErrorMessage = "Password should be at least 8 characters long")]
        [DisplayName("New password")]
        public string NewPassword { get; set; }
    }
}
