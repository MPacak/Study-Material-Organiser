using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models
{

        public class UserPasswordChangeDto
        {
            [Required]
            [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Username must contain only alphabetic characters.")]
            [Display(Name = "Username")]
            public string Username { get; set; }
            [Required]
            [Display(Name = "Old Password")]
            public string OldPassword { get; set; }
            [Required]
            [Display(Name = "New Password")]
            public string NewPassword { get; set; }
        }
    }