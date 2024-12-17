using System.ComponentModel.DataAnnotations;

namespace StudyMaterialOrganiser.ViewModels
{
    public class UserEditVM
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Username must contain only alphabetic characters.")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Username must contain only alphabetic characters.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Username must contain only alphabetic characters.")]
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
    }
}
