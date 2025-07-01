using eventLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class EventVM
    {
        public string? Username { get; set; }

        public Event? Event { get; set; } = new Event();

        public EventRegistration? EventRegistration { get; set; } = new EventRegistration();

        public EventPerformer? EventPerformer { get; set; } = new EventPerformer();

        public IEnumerable<Event>? Events { get; set; } = new List<Event>();

        public IEnumerable<EventPerformer>? EventPerformers { get; set; } = new List<EventPerformer>();

        public IEnumerable<EventType>? EventTypes { get; set; } = new List<EventType>();

        public IEnumerable<Performer>? Performers { get; set;} = new List<Performer>();
        public IEnumerable<EventRegistration>? EventRegistrations { get; set; } = new List<EventRegistration>();


    }
}
