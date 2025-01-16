using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class MaterialUser
    {
        public int IdmaterialUser { get; set; }
        public int? MaterialId { get; set; }
        public int? UserId { get; set; }

        public virtual Material? Material { get; set; }
        public virtual User? User { get; set; }
    }
}
