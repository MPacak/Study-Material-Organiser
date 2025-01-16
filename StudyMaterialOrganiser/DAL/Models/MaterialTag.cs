using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class MaterialTag
    {
        public int IdmaterialTag { get; set; }
        public int? MaterialId { get; set; }
        public int? TagId { get; set; }

        public virtual Material? Material { get; set; }
        public virtual Tag? Tag { get; set; }
    }
}
