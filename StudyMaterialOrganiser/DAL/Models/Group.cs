using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Group
{
    public int Idgroup { get; set; }

    public string Name { get; set; } = null!;

    public int? TagId { get; set; }

    public virtual Tag? Tag { get; set; }

    public virtual ICollection<UserGroup> UserGroups { get; } = new List<UserGroup>();
}
