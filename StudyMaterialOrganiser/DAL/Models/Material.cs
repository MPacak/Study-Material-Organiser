using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Material
{
    public int Idmaterial { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string Link { get; set; } = null!;

    public string FilePath { get; set; } = null!;

    public int FolderTypeId { get; set; }

    public virtual ICollection<MaterialTag> MaterialTags { get; } = new List<MaterialTag>();
}
