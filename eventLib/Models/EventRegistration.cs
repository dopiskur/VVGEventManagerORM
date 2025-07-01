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
    public class EventRegistration
    {
        [HiddenInput]
        public int? IDEventRegistration { get; set; }
        [HiddenInput]
        public int? EventID { get; set; }
        [HiddenInput]
        [Required]
        public int? UserID {  get; set; }
        [Required(ErrorMessage = "Event name is required")]
        [DisplayName("Event name")]
        public string? EventName { get; set; }
        [Required(ErrorMessage = "Event description is required")]
        [DisplayName("Description")]
        public string? Description { get; set; }
        [Required]
        [DisplayName("Category")]
        public string? EventTypeName { get; set; }
        [Required(ErrorMessage = "Date is required")]
        [DisplayName("Date")]
        public DateTime? Date { get; set; }

        public string? ImageName { get; set; }
        public byte[]? ImageData { get; set; }

        // Navigation properties for Entity Framework
        public virtual Event? Event { get; set; }
        public virtual User? User { get; set; }
    }
}
