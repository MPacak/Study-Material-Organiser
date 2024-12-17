using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models
{
    public class UserDto
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Username must contain only alphabetic characters.")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "First name must contain only alphabetic characters.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Last Name must contain only alphabetic characters.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[0-9].*[0-9].*[0-9].*[0-9].*[0-9].*[0-9].*[0-9].*[0-9].*[0-9])\+?[0-9\s\-\(\)]*$", ErrorMessage = "Phone number must contain at least 9 digits and can only contain digits, spaces, '+', '-', and '()'")]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }
        [Display(Name = "Creation Time")]
        public DateTime CreatedAt { get; set; }
        [Display(Name = "Deleted")]
        public bool? IsDeleted { get; set; }
        [Display(Name = "Role")]
        public int? Role { get; set; }
    }
}

