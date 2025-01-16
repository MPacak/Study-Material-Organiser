using StudyMaterialOrganiser.ViewModels;

namespace StudyMaterialOrganiser.Utilities
{
    public class ConfirmationManager
    {
        private static ConfirmationManager _instance;

   
        private static readonly object _lock = new object();

        
        private ConfirmationManager() { }

        
        public static ConfirmationManager GetInstance()
        {
           
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new ConfirmationManager();
                    }
                }
            }

            return _instance;
        }

        
        public ConfirmationVM CreateConfirmation(string message, string actionName, string controllerName, int redirectSeconds = 3)
        {
            return new ConfirmationVM
            {
                Message = message,
                ActionName = actionName,
                ControllerName = controllerName,
                RedirectSeconds = redirectSeconds
            };
        }
    }
}
