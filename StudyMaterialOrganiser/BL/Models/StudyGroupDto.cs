using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BL.Models
{
	public class StudyGroupDto
	{
		[Required]
		[Display(Name = "Study Group ID")]
		public int Id { get; set; }

		[Required]
		[Display(Name = "Name")]
		public string Name { get; set; }

		[Required]
		[Display(Name = "Tag ID")]
		public int TagId { get; set; }

		[Display(Name = "Tag Name")]
		public string? TagName { get; set; } 

		[DisplayName("Users")]
		public List<string> Users { get; set; } = new List<string>(); 
	}
}