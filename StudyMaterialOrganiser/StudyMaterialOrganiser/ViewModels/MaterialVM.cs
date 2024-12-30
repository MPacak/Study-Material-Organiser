using BL.Models;
using DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace StudyMaterialOrganiser.ViewModels
{
    public class MaterialVM
    {
        public int Idmaterial { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public string Link { get; set; } = string.Empty;
        [Required]
        public string FilePath { get; set; } = string.Empty;

        [Required]
        public int FolderTypeId { get; set; }
        public string FolderTypeName => Enum.GetName(typeof(FileType), FolderTypeId) ?? "Unknown";
        [Required]
        public IFormFile File { get; set; }
        public List<int> SelectedTagIds { get; set; } = new List<int>();
        public List<TagVM>? AvailableTags { get; set; } = new List<TagVM>();
        public List<string> TagNames { get; set; }
    }
}
