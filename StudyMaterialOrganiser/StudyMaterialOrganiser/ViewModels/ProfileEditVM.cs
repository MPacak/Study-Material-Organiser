using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ViewModels;

public class ProfileEditVM
{
    [Required]
    public int Id { get; set; }
    [Required]
    [Display(Name = "Username")]
    public string Username { get; set; }

    [Required]

    [Display(Name = "First Name")]
    public string FirstName { get; set; }

    [Required]
    [Display(Name = "Last Name")]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }
    [Required]
    [Phone]
    [Display(Name = "Phone Number")]
    public string Phone { get; set; }
    public int Role { get; set; }
    [Required]
    [Display(Name = "Password")]
    public string Password { get; set; }
}