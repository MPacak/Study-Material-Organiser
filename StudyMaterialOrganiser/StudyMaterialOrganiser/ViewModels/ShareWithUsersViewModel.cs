namespace StudyMaterialOrganiser.ViewModels
{
    public class ShareWithUsersViewModel
    {
        public int MaterialId { get; set; }
        public string MaterialLink { get; set; }
        public string SearchTerm { get; set; }
        public List<UserShareViewModel> Users { get; set; }
    }
}
