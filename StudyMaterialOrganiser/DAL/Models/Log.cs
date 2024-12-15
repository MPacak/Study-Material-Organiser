using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Log
{
    public int Idlog { get; set; }

    public string Content { get; set; } = null!;

    public int? UserId { get; set; }

    public DateTime? Timestamp { get; set; }

    public virtual User? User { get; set; }
}
