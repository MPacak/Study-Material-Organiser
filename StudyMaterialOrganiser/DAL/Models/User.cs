using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class User
{
    public int Iduser { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public int? RoleId { get; set; }

    public virtual ICollection<Log> Logs { get; } = new List<Log>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<UserGroup> UserGroups { get; } = new List<UserGroup>();
}
