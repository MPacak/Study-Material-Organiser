﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class MaterialTag
{
    public int IdmaterialTag { get; set; }

    public int? MaterialId { get; set; }

    public int TagId { get; set; }

    public virtual Material Material { get; set; }

    public virtual Tag Tag { get; set; }
}