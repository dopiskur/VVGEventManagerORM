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
    public class UserLogin
    {
        [Required(ErrorMessage = "User name is required")]
        [DisplayName("Username")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password name is required")]
        [DisplayName("Password")]
        public string Password { get; set; }

    }
}
