using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models
{
    public class MaterialTagDto
    {
        public int Id { get; set; }
        public int? MaterialId { get; set; }
        public int? TagId { get; set; }
    }
}
