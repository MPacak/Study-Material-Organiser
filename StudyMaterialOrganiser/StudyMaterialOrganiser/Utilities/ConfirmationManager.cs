using StudyMaterialOrganiser.ViewModels;

namespace StudyMaterialOrganiser.Utilities
{
    public class ConfirmationManager
    {
        private static ConfirmationManager _instance;

        // Lock object to ensure thread safety
        private static readonly object _lock = new object();

        // Private constructor to prevent instantiation
        private ConfirmationManager() { }

        // Public method to access the single instance
        public static ConfirmationManager GetInstance()
        {
            // Double-check locking mechanism for thread safety
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

        // Method to create a ConfirmationVM
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
