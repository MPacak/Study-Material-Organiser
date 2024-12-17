using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models
{
    public class UserRegistrationDto
    {
        [Required(ErrorMessage = "Username is required.")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Username must contain only alphabetic characters.")]
        [DisplayName("Username")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [DisplayName("Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "First Name must contain only alphabetic characters.")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is required.")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Last Name must contain only alphabetic characters.")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        [DisplayName("Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Phone is required.")]
        [RegularExpression(@"^(?=.*[0-9].*[0-9].*[0-9].*[0-9].*[0-9].*[0-9].*[0-9].*[0-9].*[0-9])\+?[0-9\s\-\(\)]*$", ErrorMessage = "Phone number must contain at least 9 digits and can only contain digits, spaces, '+', '-', and '()'")]
        [DisplayName("Phone Number")]
        public string Phone { get; set; }
        [DisplayName("Role")]
        public int Role { get; set; }

    }
}