﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Tag
{
    public int Idtag { get; set; }

    public string TagName { get; set; }

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    public virtual ICollection<MaterialTag> MaterialTags { get; set; } = new List<MaterialTag>();
}