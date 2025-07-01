using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eventLib.Models
{
    public class Performer
    {
        [HiddenInput]
        public int? IDPerformer { get; set; }
        [Required(ErrorMessage = "Performer name is required")]
        [DisplayName("Performer name")]
        public string? PerformerName { get; set; }

        // Navigation properties for Entity Framework
        public virtual ICollection<EventPerformer> EventPerformers { get; set; } = new List<EventPerformer>();
    }
}
