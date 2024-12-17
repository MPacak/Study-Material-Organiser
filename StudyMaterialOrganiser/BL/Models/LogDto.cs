using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models
{
    public class LogDto
    {
        [Required]
        [Display(Name = "Log Id")]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Level")]
        public string Level { get; set; }
        [Required]
        [Display(Name = "Message")]
        public string Message { get; set; }
        [Required]
        [Display(Name = "Timestamp")]
        public DateTime Timestamp { get; set; }

    }
}
