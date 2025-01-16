using BL.Models;

namespace StudyMaterialOrganiser.ViewModels
{
    public class UserShareViewModel : UserDto
    {
        public string Permission { get; set; } = "None";
    }
}
