using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Log
    {
        public int Id { get; set; }
        public string Message { get; set; } = null!;
        public string Level { get; set; } = null!;
        public DateTime Timestamp { get; set; }
    }
}
