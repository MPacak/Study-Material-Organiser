using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models
{
    public class TagDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<string> Groups { get; set; } = new List<string>();
      //  public List<string> MaterialTags { get; set; } = new List<string>();
    }
}
