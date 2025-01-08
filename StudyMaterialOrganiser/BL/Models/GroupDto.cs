using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models
{
    public class GroupDto
    {
        [Required]
        [Display(Name = "Group Id")]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string? Name { get; set; }
        [Required]
        [Display(Name = "Tag")]
        public string? TagName { get; set; }
        [DisplayName("Skills")]
		public List<string> Skills { get; set; } = new List<string>();
        [DisplayName("Users")]
        public List<string>? Users { get; set; } = new List<string>();

	}
}
