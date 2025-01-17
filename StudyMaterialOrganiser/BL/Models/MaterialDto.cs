using DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models
{
   public class MaterialDto
    {
        public int Idmaterial { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string? Description { get; set; }

        
        public string Link { get; set; } = string.Empty;

        
        public string FilePath { get; set; } = string.Empty;

       
        public int FolderTypeId { get; set; }

        public List<int> TagIds { get; set; } = new List<int>();
        
        public List<string>? TagNames { get; set; }  

    }
}
