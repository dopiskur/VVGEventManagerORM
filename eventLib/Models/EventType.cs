using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eventLib.Models
{
    public class EventType
    {
        public int? IDEventType { get; set; }
        [Required]
        public string? EventTypeName {  get; set; }
    }
}
