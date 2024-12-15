using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Role
{
    public int Idrole { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; } = new List<User>();
}
