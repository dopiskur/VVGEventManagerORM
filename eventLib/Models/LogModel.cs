using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eventLib.Models
{
    public class LogModel
    {
        public int? IDLog {  get; set; }
        public int? IDLevel { get; set; }
        public string? LevelName { get; set; }
        public string? InfoMessage { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}
