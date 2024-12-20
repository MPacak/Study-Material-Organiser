namespace StudyMaterialOrganiser.ViewModels
{
    public class TagVM
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<string> Groups { get; set; } = new List<string>();
       // public List<string> MaterialTags { get; set; } = new List<string>();
    }
}
