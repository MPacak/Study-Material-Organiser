using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Material
    {
        public Material()
        {
            MaterialTags = new HashSet<MaterialTag>();
            MaterialUsers = new HashSet<MaterialUser>();
        }

        public int Idmaterial { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string Link { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public int FolderTypeId { get; set; }

        public virtual ICollection<MaterialTag> MaterialTags { get; set; }
        public virtual ICollection<MaterialUser> MaterialUsers { get; set; }
    }
}
