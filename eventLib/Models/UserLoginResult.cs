using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eventLib.Models
{
    public class UserLoginResult
    {
        public int? IDUser { get; set; }
        [DisplayName("Username")]
        public string? Username { get; set; }
        [DisplayName("Token")]
        public string? Token { get; set; }
    }
}
