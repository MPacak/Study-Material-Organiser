namespace StudyMaterialOrganiser.ViewModels
{
    public class ConfirmationVM
    {
        public string Message { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public int RedirectSeconds { get; set; } = 3;
    }
}
