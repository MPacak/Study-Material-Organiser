using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models
{
    public class UserGroupDto
    {
		[Display(Name = "Group Id")]
		public int? Id { get; set; }
		[Required]
		[Display(Name = "User Id")]
		public int UserId { get; set; }
		[Required]
		[Display(Name = "Group Id")]
		public int GroupId { get; set; }
		
		[Required]
		[Display(Name = "Group Name")]
		public string GroupName { get; set; }
		[Required]
		[Display(Name = "GroupTag Name")]
		public string GroupTagName { get; set; }
		[Display(Name = "Username")]
		public string UserUsername { get; set; }

	}
}
