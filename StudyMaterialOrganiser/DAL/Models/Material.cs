﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Material
{
    public int Idmaterial { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Link { get; set; }

    public string FilePath { get; set; }

    public int FolderTypeId { get; set; }

    public virtual ICollection<MaterialTag> MaterialTags { get; set; } = new List<MaterialTag>();
}