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
    public class Event
    {
        [HiddenInput]
        public int? IDEvent {  get; set; }
        [Required(ErrorMessage = "Event name is required")]
        [DisplayName("Event name")]
        public string? EventName { get; set; }
        [Required(ErrorMessage = "Event date is required")]
        [DisplayName("Date")]
        public DateTime? Date {  get; set; }
        [Required(ErrorMessage = "event description is required")]
        [DisplayName("Description")]
        public string? Description { get; set; }
        [Required]
        [DisplayName("Category")]
        public int? eventTypeID { get; set; }
        [DisplayName("Category")]
        [Required]
        public string? EventTypeName { get; set; }
        [HiddenInput]
        public int? ImageID { get; set; }
        public string? ImageName { get; set; }
        public byte[]? ImageData { get; set; }

        // Navigation properties for Entity Framework
        public virtual EventType? EventType { get; set; }
        public virtual ICollection<EventPerformer> EventPerformers { get; set; } = new List<EventPerformer>();
        public virtual ICollection<EventRegistration> EventRegistrations { get; set; } = new List<EventRegistration>();
        public virtual Image? Image { get; set; }
    }
}
