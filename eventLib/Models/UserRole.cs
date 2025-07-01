using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace eventLib.Models;

public partial class UserRole
{
    public int? IDUserRole { get; set; }
    [Required(ErrorMessage = "Role name is required")]
    [DisplayName("Role name")]
    public string? RoleName { get; set; } = null!;

    public virtual ICollection<User> Users { get; } = new List<User>();
}
