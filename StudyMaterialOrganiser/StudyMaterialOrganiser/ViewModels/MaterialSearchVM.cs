using DAL.Models;

namespace StudyMaterialOrganiser.ViewModels
{
    public class MaterialSearchVM
    {
        public List<MaterialVM> Materials { get; set; } = new List<MaterialVM>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public string? Query { get; set; }
        public int? FileType { get; set; }
        public List<int>? TagIds { get; set; }

        public List<TagVM>? AvailableTags { get; set; }
        public string? NotificationMessage { get; set; }
        public Dictionary<int, string> AvailableFileTypes =>
            Enum.GetValues(typeof(FileType))
                .Cast<FileType>()
                .ToDictionary(t => (int)t, t => t.ToString());
    }
}
