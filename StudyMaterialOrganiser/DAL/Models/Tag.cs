using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Tag
{
    public int Idtag { get; set; }

    public string TagName { get; set; } = null!;

    public virtual ICollection<Group> Groups { get; } = new List<Group>();

    public virtual ICollection<MaterialTag> MaterialTags { get; } = new List<MaterialTag>();
}
