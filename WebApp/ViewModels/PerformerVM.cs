using eventLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class PerformerVM
    {
        public Performer? Performer { get; set; } = new Performer();
        public IEnumerable<Performer> Performers { get; set; } = new List<Performer>();
    }
}
