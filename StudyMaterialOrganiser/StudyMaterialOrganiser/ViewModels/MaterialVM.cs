using System.ComponentModel.DataAnnotations;

namespace StudyMaterialOrganiser.ViewModels
{
    public class MaterialVM
    {
        public int Id { get; set; }
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

        public List<string> Tags { get; set; } = new List<string>();
    }
}
