using System.Security.Claims;
using AutoMapper;
using BL.IServices;
using BL.Models;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using StudyMaterialOrganiser.Controllers.UserModule;
using StudyMaterialOrganiser.ViewModels;

namespace StudyMaterialOrganiser.Test.UserTests
{
	public class UserControllerIntegrationTests : IClassFixture<TestFixture>
	{
		private readonly TestFixture _fixture;
		private readonly UserController _controller;
		private readonly Mock<ILogger<UserController>> _mockLogger;

		public UserControllerIntegrationTests(TestFixture fixture)
		{
			_fixture = fixture;
			_mockLogger = new Mock<ILogger<UserController>>();
			_controller = SetupController();
			SetupTestEnvironment();
		}

		private UserController SetupController()
		{
			return new UserController(
				_fixture.ServiceProvider.GetRequiredService<IUserService>(),
				_fixture.ServiceProvider.GetRequiredService<IMapper>(),
				_mockLogger.Object,
				_fixture.ServiceProvider.GetRequiredService<ILogService>(),
				_fixture.ServiceProvider.GetRequiredService<IAuthService>(),
				_fixture.ServiceProvider.GetRequiredService<IRoleService>()
			);
		}

		private void SetupTestEnvironment()
		{
			SetupAdminUser();
			CleanupTestData();
			SeedTestData();
		}

		private void SeedTestData()
		{
			var dbContext = _fixture.DbContext;
			var testUser = new User
			{
				Username = "testuser",
				Email = "test@test.com",
				FirstName = "Test",
				LastName = "User",
				Role = 1,
				PwdHash = "hash",
				PwdSalt = "salt",
				IsDeleted = false,
				SecurityToken = "token123"
			};
			dbContext.Users.Add(testUser);
			dbContext.SaveChanges();
		}

		private void CleanupTestData()
		{
			var dbContext = _fixture.DbContext;
			dbContext.Users.RemoveRange(dbContext.Users);
			dbContext.SaveChanges();
		}

		[Fact]
		public void List_ReturnsViewWithCorrectUsers()
		{
			var result = _controller.List();
			var viewResult = Assert.IsType<ViewResult>(result);
			var users = Assert.IsAssignableFrom<ICollection<UserDto>>(viewResult.Model);

			Assert.Single(users);
			var user = users.First();
			Assert.Equal("testuser", user.Username);
			Assert.Equal("test@test.com", user.Email);
		}

		[Fact]
		public void Create_Post_ValidModel_CreatesUserInDatabase()
		{
			SetupTempData();
			var userRegistration = new UserRegistrationDto
			{
				UserName = "newuser",
				Email = "new@test.com",
				Password = "Password123!",
				FirstName = "New",
				LastName = "User",
				Role = 1
			};

			var result = _controller.Create(userRegistration);
			var redirectResult = Assert.IsType<RedirectToActionResult>(result);
			Assert.Equal(nameof(_controller.List), redirectResult.ActionName);

			var savedUser = _fixture.DbContext.Users
				.FirstOrDefault(u => u.Username == userRegistration.UserName);
			Assert.NotNull(savedUser);
			Assert.Equal(userRegistration.Email, savedUser.Email);
			Assert.Equal(userRegistration.FirstName, savedUser.FirstName);
		}

		[Fact]
		public void Create_ShouldFailWithExistingUsername()
		{
			var mockUserService = new Mock<IUserService>();
			mockUserService.Setup(s => s.Create(It.IsAny<UserRegistrationDto>()))
				.Throws(new Exception("Username already exists"));

			var controller = new UserController(
				mockUserService.Object,
				_fixture.ServiceProvider.GetRequiredService<IMapper>(),
				_mockLogger.Object,
				_fixture.ServiceProvider.GetRequiredService<ILogService>(),
				_fixture.ServiceProvider.GetRequiredService<IAuthService>(),
				_fixture.ServiceProvider.GetRequiredService<IRoleService>()
			);

			SetupTempData(controller);

			var userRegistration = new UserRegistrationDto
			{
				UserName = "testuser",
				Email = "new@test.com",
				Password = "Password123!"
			};

			var result = controller.Create(userRegistration);

			var viewResult = Assert.IsType<ViewResult>(result);
			Assert.Equal("Username already exists", controller.TempData["ToastMessage"]);
			Assert.Equal("error", controller.TempData["ToastType"]);
		}

		[Fact]
		public void Edit_Post_ValidModel_UpdatesUserInDatabase()
		{
			SetupTempData();
			var dbContext = _fixture.DbContext;
			var user = dbContext.Users.First();

			var editModel = new UserEditVM
			{
				Id = user.Id,
				Username = "updated",
				Email = "updated@test.com",
				FirstName = "Updated",
				LastName = "User"
			};

			var result = _controller.Edit(editModel);
			var updatedUser = dbContext.Users.Find(user.Id);
			Assert.NotNull(updatedUser);
			Assert.Equal("updated", updatedUser.Username);
			Assert.Equal("updated@test.com", updatedUser.Email);
		}

		[Fact]
		public void Edit_ShouldFailWithInvalidId()
		{
			var mockUserService = new Mock<IUserService>();
			mockUserService.Setup(s => s.GetById(-1)).Throws(new Exception("User not found"));

			var controller = new UserController(
				mockUserService.Object,
				_fixture.ServiceProvider.GetRequiredService<IMapper>(),
				_mockLogger.Object,
				_fixture.ServiceProvider.GetRequiredService<ILogService>(),
				_fixture.ServiceProvider.GetRequiredService<IAuthService>(),
				_fixture.ServiceProvider.GetRequiredService<IRoleService>()
			);
			SetupTempData(controller);

			var result = controller.Edit(-1);

			var redirectResult = Assert.IsType<RedirectToActionResult>(result);
			Assert.Equal("List", redirectResult.ActionName);
		}

		[Fact]
		public void Delete_Post_MarksUserAsDeletedInDatabase()
		{
			var dbContext = _fixture.DbContext;
			var user = dbContext.Users.First();
			var userDto = new UserDto
			{
				Id = user.Id,
				Username = user.Username
			};

			var result = _controller.Delete(userDto);
			var deletedUser = dbContext.Users.Find(user.Id);
			Assert.True(deletedUser.IsDeleted);
			Assert.NotNull(deletedUser.DeletedAt);
		}
		[Fact]
		public void Delete_ShouldFailWithInvalidUser()
		{
			// Arrange
			var mockUserService = new Mock<IUserService>();
			mockUserService.Setup(s => s.Delete(-1))
				.Throws(new Exception("Test error"));

			var controller = new UserController(
				mockUserService.Object,
				_fixture.ServiceProvider.GetRequiredService<IMapper>(),
				_mockLogger.Object,
				_fixture.ServiceProvider.GetRequiredService<ILogService>(),
				_fixture.ServiceProvider.GetRequiredService<IAuthService>(),
				_fixture.ServiceProvider.GetRequiredService<IRoleService>()
			);

			controller.TempData = new TempDataDictionary(
				new DefaultHttpContext(),
				Mock.Of<ITempDataProvider>()
			);

			var userDto = new UserDto { Id = -1 };
			var result = controller.Delete(userDto);

			var redirectResult = Assert.IsType<ViewResult>(result);
			Assert.Equal("Test error", controller.TempData["ToastMessage"]);
			Assert.Equal("error", controller.TempData["ToastType"]);
		}

		[Fact]
		public void LoadUserList_ReturnsPartialView()
		{
			var result = _controller.LoadUserList();
			var partialViewResult = Assert.IsType<PartialViewResult>(result);
			Assert.Equal("_UserList", partialViewResult.ViewName);

			var users = Assert.IsAssignableFrom<ICollection<UserDto>>(partialViewResult.Model);
			Assert.NotEmpty(users);
		}

		[Fact]
		public void Registration_Get_ReturnsView()
		{
			var result = _controller.Registration();
			Assert.IsType<ViewResult>(result);
		}

		[Fact]
		public void CreateUser_ValidModel_CreatesUserAndReturnsView()
		{
			var userRegistration = new UserRegistrationDto
			{
				UserName = "newuser",
				Email = "new@test.com",
				Password = "Password123!",
				FirstName = "New",
				LastName = "User"
			};

			var result = _controller.CreateUser(userRegistration);

			var viewResult = Assert.IsType<ViewResult>(result);
			Assert.NotNull(viewResult.Model);
		}

		[Fact]
		public void Login_Get_ReturnsView()
		{
			var result = _controller.LogIn();
			Assert.IsType<ViewResult>(result);
		}

		[Fact]
		public void Login_Post_ValidCredentials_RedirectsToHomeAndAuthenticates()
		{
			var mockAuthService = new Mock<IAuthService>();
			var user = new User
			{
				Id = 1,
				Username = "testuser",
				Role = 1,
				FirstName = "Test",
				LastName = "User",
				Email = "test@test.com",
				PwdHash = "hash",
				PwdSalt = "salt",
				SecurityToken = "token",
				IsDeleted = false
			};

			mockAuthService.Setup(x => x.Authenticate(It.IsAny<string>(), It.IsAny<string>()))
						  .Returns(user);

			var controller = SetupControllerWithMockAuth(mockAuthService.Object);
			SetupTempData(controller);

			var loginDto = new UserLoginDto
			{
				Username = "testuser",
				Password = "Password123!"
			};

			var result = controller.LogIn(loginDto);

			var redirectResult = Assert.IsType<RedirectToActionResult>(result);
			Assert.Equal("Index", redirectResult.ActionName);
			Assert.Equal("Home", redirectResult.ControllerName);
			mockAuthService.Verify(x => x.SignIn(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
		}

		[Fact]
		public void Login_ShouldFailWithInvalidCredentials()
		{
			var mockAuthService = new Mock<IAuthService>();
			mockAuthService.Setup(x => x.Authenticate(It.IsAny<string>(), It.IsAny<string>()))
				.Returns((User)null);

			var controller = SetupControllerWithMockAuth(mockAuthService.Object);
			SetupTempData(controller);
			controller.ModelState.AddModelError("", "Invalid credentials");

			var loginDto = new UserLoginDto
			{
				Username = "nonexistent",
				Password = "wrong"
			};

			var result = controller.LogIn(loginDto);

			var viewResult = Assert.IsType<ViewResult>(result);
			Assert.False(controller.ModelState.IsValid);
		}


		[Fact]
		public void Logout_RedirectsToHome()
		{
			var mockAuthService = new Mock<IAuthService>();
			var controller = SetupControllerWithMockAuth(mockAuthService.Object);
			SetupTestHttpContext(controller);
			SetupTempData(controller);

			var result = controller.Logout();

			var redirectResult = Assert.IsType<RedirectToActionResult>(result);
			Assert.Equal("Index", redirectResult.ActionName);
			Assert.Equal("Home", redirectResult.ControllerName);
			mockAuthService.Verify(x => x.SignOut(), Times.Once);
		}

		[Fact]
		public void ProfileDetails_ReturnsViewWithUserData()
		{
			SetupTestHttpContext(_controller);

			var result = _controller.ProfileDetails();

			var viewResult = Assert.IsType<ViewResult>(result);
			Assert.NotNull(viewResult.Model);
		}

		[Fact]
		public void ProfileDetailsPartial_ReturnsPartialView()
		{
			var result = _controller.ProfileDetailsPartial();

			var partialViewResult = Assert.IsType<PartialViewResult>(result);
			Assert.Equal("_ProfileDetailsPartial", partialViewResult.ViewName);
		}

		[Fact]
		public void ProfileEdit_Get_ReturnsPartialView()
		{
			var dbContext = _fixture.DbContext;
			var user = dbContext.Users.First();

			var result = _controller.ProfileEdit(user.Id);

			var partialViewResult = Assert.IsType<PartialViewResult>(result);
			Assert.Equal("_EditModalPartial", partialViewResult.ViewName);
		}

		[Fact]
		public void ProfileEdit_Post_ValidModel_ReturnsUpdatedPartialView()
		{
			var mockAuthService = new Mock<IAuthService>();
			var mockUserService = new Mock<IUserService>();

			var user = new UserDto
			{
				Id = 1,
				Username = "testuser",
				Email = "test@test.com",
				FirstName = "Test",
				LastName = "User",
				Role = 1
			};

			mockUserService.Setup(x => x.GetById(It.IsAny<int>())).Returns(user);
			mockUserService.Setup(x => x.GetByUserName(It.IsAny<string>())).Returns(user);
			mockUserService.Setup(x => x.GetByEmail(It.IsAny<string>())).Returns((UserDto)null);

			var controller = new UserController(
				mockUserService.Object,
				_fixture.ServiceProvider.GetRequiredService<IMapper>(),
				_mockLogger.Object,
				_fixture.ServiceProvider.GetRequiredService<ILogService>(),
				mockAuthService.Object,
				_fixture.ServiceProvider.GetRequiredService<IRoleService>()
			);

			SetupTempData(controller);

			var userDto = new UserDto
			{
				Id = 1,
				Username = "updated",
				Email = "updated@test.com",
				FirstName = "Updated",
				LastName = "User",
				Role = 1
			};

			var result = controller.ProfileEdit(1, userDto);

			var partialViewResult = Assert.IsType<PartialViewResult>(result);
			Assert.Equal("_ProfileDetailsPartial", partialViewResult.ViewName);
			mockUserService.Verify(x => x.Update(It.IsAny<int>(), It.IsAny<UserDto>()), Times.Once);
		}

		private void SetupTempData(UserController controller = null)
		{
			var target = controller ?? _controller;
			target.TempData = new TempDataDictionary(
				new DefaultHttpContext(),
				Mock.Of<ITempDataProvider>()
			);
		}

		private void SetupTestHttpContext(UserController controller)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, "testuser"),
				new Claim(ClaimTypes.Role, "Admin")
			};
			var identity = new ClaimsIdentity(claims, "TestAuth");
			var claimsPrincipal = new ClaimsPrincipal(identity);
			var httpContext = new DefaultHttpContext { User = claimsPrincipal };

			controller.ControllerContext = new ControllerContext
			{
				HttpContext = httpContext
			};
		}

		private void SetupAdminUser()
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, "testAdmin"),
				new Claim(ClaimTypes.Role, "Admin")
			};
			var identity = new ClaimsIdentity(claims, "TestAuth");
			var claimsPrincipal = new ClaimsPrincipal(identity);

			_controller.ControllerContext = new ControllerContext
			{
				HttpContext = new DefaultHttpContext { User = claimsPrincipal }
			};
		}

		private UserController SetupControllerWithMockAuth(IAuthService authService)
		{
			return new UserController(
				_fixture.ServiceProvider.GetRequiredService<IUserService>(),
				_fixture.ServiceProvider.GetRequiredService<IMapper>(),
				_mockLogger.Object,
				_fixture.ServiceProvider.GetRequiredService<ILogService>(),
				authService,
				_fixture.ServiceProvider.GetRequiredService<IRoleService>()
			);
		}
	}
}
