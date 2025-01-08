using BL.Models;
using DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace StudyMaterialOrganiser.ViewModels
{
    public class MaterialVM : MaterialDto
    {

        public string FolderTypeName => Enum.GetName(typeof(FileType), FolderTypeId) ?? "Unknown";
        [Required]
        public IFormFile File { get; set; }
       // public List<int> SelectedTagIds { get; set; } = new List<int>();
        public List<TagVM>? AvailableTags { get; set; } = new List<TagVM>();

    }
}
