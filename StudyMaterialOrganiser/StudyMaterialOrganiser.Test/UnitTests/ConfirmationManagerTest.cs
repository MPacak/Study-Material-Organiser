using StudyMaterialOrganiser.Utilities;
using StudyMaterialOrganiser.ViewModels;

namespace StudyMaterialOrganiser.Test.UnitTests
{
    public class ConfirmationManagerTests
    {
        [Fact]
        public void GetInstance_ReturnsSingletonInstance()
        {
            
            var instance1 = ConfirmationManager.GetInstance();
            var instance2 = ConfirmationManager.GetInstance();

            
            Assert.NotNull(instance1);
            Assert.Same(instance1, instance2); // Ensure both references point to the same instance
        }

        [Fact]
        public void CreateConfirmation_ReturnsCorrectViewModel()
        {
            
            var confirmationManager = ConfirmationManager.GetInstance();
            var expectedMessage = "Test Message";
            var expectedActionName = "TestAction";
            var expectedControllerName = "TestController";
            var expectedRedirectSeconds = 5;

            
            var confirmation = confirmationManager.CreateConfirmation(
                expectedMessage,
                expectedActionName,
                expectedControllerName,
                expectedRedirectSeconds
            );

            
            Assert.NotNull(confirmation);
            Assert.IsType<ConfirmationVM>(confirmation);
            Assert.Equal(expectedMessage, confirmation.Message);
            Assert.Equal(expectedActionName, confirmation.ActionName);
            Assert.Equal(expectedControllerName, confirmation.ControllerName);
            Assert.Equal(expectedRedirectSeconds, confirmation.RedirectSeconds);
        }

        [Fact]
        public void CreateConfirmation_DefaultRedirectSecondsIsThree()
        {
            
            var confirmationManager = ConfirmationManager.GetInstance();
            var expectedMessage = "Test Message";
            var expectedActionName = "TestAction";
            var expectedControllerName = "TestController";

            
            var confirmation = confirmationManager.CreateConfirmation(
                expectedMessage,
                expectedActionName,
                expectedControllerName
            );

            
            Assert.NotNull(confirmation);
            Assert.Equal(3, confirmation.RedirectSeconds); 
        }
    }

}
