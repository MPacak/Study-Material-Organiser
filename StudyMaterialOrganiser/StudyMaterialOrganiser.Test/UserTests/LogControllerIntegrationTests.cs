using AutoMapper;
using BL.IServices;
using BL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using StudyMaterialOrganiser.Controllers.UserModule;
using Xunit;
using DAL.Models;
using Microsoft.Extensions.Logging;

namespace StudyMaterialOrganiser.Test.LogTests
{
	public class LogControllerIntegrationTests : IClassFixture<TestFixture>
	{
		private readonly TestFixture _fixture;
		private readonly LogController _controller;
		private readonly Mock<ILogger<UserController>> _mockLogger;
		private readonly Mock<ILogService> _mockLogService;

		public LogControllerIntegrationTests(TestFixture fixture)
		{
			_fixture = fixture;
			_mockLogger = new Mock<ILogger<UserController>>();
			_mockLogService = new Mock<ILogService>();

			_controller = new LogController(
				_fixture.ServiceProvider.GetRequiredService<IMapper>(),
				_mockLogger.Object,
				_mockLogService.Object
			);

			_controller.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(
				new DefaultHttpContext(),
				Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>()
			);
		}

		[Fact]
		public void List_WithInvalidInput_RedirectsToError()
		{
			// Arrange
			_mockLogService.Setup(s => s.GetPaginatedLogs(It.IsAny<int>(), It.IsAny<int>()))
						 .Throws(new Exception("Test exception"));

			// Act
			var result = _controller.List(-1, 0);

			// Assert
			var redirectResult = Assert.IsType<RedirectToActionResult>(result);
			Assert.Equal("Error", redirectResult.ActionName);
			Assert.Equal("Home", redirectResult.ControllerName);
			Assert.Equal("An error occurred while processing your request.", _controller.TempData["ToastMessage"]);
			Assert.Equal("error", _controller.TempData["ToastType"]);
		}
	}
}