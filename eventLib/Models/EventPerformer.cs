using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eventLib.Models
{
    public class EventPerformer
    {
        [Required]
        public int? IDEventPerformer {  get; set; }
        [Required]
        public int? EventID { get; set; }
        [Required]
        public int? PerformerID { get; set; }
        [Required(ErrorMessage = "Performer name is required")]
        [DisplayName("Performer name")]
        public string? PerformerName {  get; set; }

        // Navigation properties for Entity Framework
        public virtual Event? Event { get; set; }
        public virtual Performer? Performer { get; set; }
    }
}
