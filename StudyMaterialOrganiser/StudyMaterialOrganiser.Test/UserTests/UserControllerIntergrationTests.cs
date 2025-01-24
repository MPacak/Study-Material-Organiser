using AutoMapper;
using BL.IServices;
using BL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using StudyMaterialOrganiser.Controllers.UserModule;
using StudyMaterialOrganiser.ViewModels;
using System.Security.Claims;
using Xunit;
using DAL.Models;
using Microsoft.Extensions.Logging;

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

			// Setup services
			_controller = new UserController(
				_fixture.ServiceProvider.GetRequiredService<IUserService>(),
				_fixture.ServiceProvider.GetRequiredService<IMapper>(),
				_mockLogger.Object,
				_fixture.ServiceProvider.GetRequiredService<ILogService>(),
				_fixture.ServiceProvider.GetRequiredService<IAuthService>(),
				_fixture.ServiceProvider.GetRequiredService<IRoleService>()
			);

			// Setup identity
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

			// Initialize test data
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
		public void List_ReturnsViewWithUsers()
		{
			var result = _controller.List();

			var viewResult = Assert.IsType<ViewResult>(result);
			var model = Assert.IsAssignableFrom<ICollection<UserDto>>(viewResult.Model);
			Assert.NotNull(model);
		}

		[Fact]
		public void Create_Post_ValidModel_RedirectsToList()
		{
			// Setup TempData
			_controller.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(
				new DefaultHttpContext(),
				Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>()
			);

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
		}

		[Fact]
		public void Edit_Get_ReturnsViewWithUser()
		{
			var dbContext = _fixture.DbContext;
			var user = dbContext.Users.First();

			var result = _controller.Edit(user.Id);

			var viewResult = Assert.IsType<ViewResult>(result);
			var model = Assert.IsType<UserEditVM>(viewResult.Model);
			Assert.Equal(user.Id, model.Id);
		}

		public void Edit_Post_ValidModel_RedirectsToList()
		{
			// Setup TempData
			_controller.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(
				new DefaultHttpContext(),
				Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>()
			);

			// Setup ModelState
			_controller.ModelState.Clear();

			var dbContext = _fixture.DbContext;
			var user = dbContext.Users.First();

			var editModel = new UserEditVM
			{
				Id = user.Id,
				Username = "updated",
				Email = "updated@test.com",
				FirstName = "Updated",
				LastName = "User",
			};

			var result = _controller.Edit(editModel);

			var redirectResult = Assert.IsType<RedirectToActionResult>(result);
			Assert.Equal(nameof(_controller.List), redirectResult.ActionName);
		}


		[Fact]
		public void Delete_Post_RedirectsToList()
		{
			var dbContext = _fixture.DbContext;
			var user = dbContext.Users.First();

			var userDto = new UserDto
			{
				Id = user.Id,
				Username = user.Username,
				FirstName = user.FirstName,
				LastName = user.LastName
			};

			var result = _controller.Delete(userDto);

			var redirectResult = Assert.IsType<RedirectToActionResult>(result);
			Assert.Equal(nameof(_controller.List), redirectResult.ActionName);
		}
		[Fact]
		public void LoadUserList_ReturnsPartialView()
		{
			var result = _controller.LoadUserList();

			var partialViewResult = Assert.IsType<PartialViewResult>(result);
			Assert.Equal("_UserList", partialViewResult.ViewName);
			Assert.NotNull(partialViewResult.Model);
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
		public void Login_Post_ValidCredentials_RedirectsToHome()
		{
			// Setup Authentication service mock
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

			var controller = new UserController(
				_fixture.ServiceProvider.GetRequiredService<IUserService>(),
				_fixture.ServiceProvider.GetRequiredService<IMapper>(),
				_mockLogger.Object,
				_fixture.ServiceProvider.GetRequiredService<ILogService>(),
				mockAuthService.Object,
				_fixture.ServiceProvider.GetRequiredService<IRoleService>()
			);

			controller.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(
				new DefaultHttpContext(),
				Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>()
			);

			var loginDto = new UserLoginDto
			{
				Username = "testuser",
				Password = "Password123!"
			};

			// Act
			var result = controller.LogIn(loginDto);

			// Assert
			var redirectResult = Assert.IsType<RedirectToActionResult>(result);
			Assert.Equal("Index", redirectResult.ActionName);
			Assert.Equal("Home", redirectResult.ControllerName);
			mockAuthService.Verify(x => x.SignIn(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
		}
		[Fact]
		public void Logout_RedirectsToHome()
		{
			// Setup Authentication service mock
			var mockAuthService = new Mock<IAuthService>();

			var controller = new UserController(
				_fixture.ServiceProvider.GetRequiredService<IUserService>(),
				_fixture.ServiceProvider.GetRequiredService<IMapper>(),
				_mockLogger.Object,
				_fixture.ServiceProvider.GetRequiredService<ILogService>(),
				mockAuthService.Object,
				_fixture.ServiceProvider.GetRequiredService<IRoleService>()
			);

			// Setup HttpContext with User identity
			var httpContext = new DefaultHttpContext();
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, "testuser"),
				new Claim(ClaimTypes.Role, "Admin")
			};
			var identity = new ClaimsIdentity(claims, "TestAuth");
			var claimsPrincipal = new ClaimsPrincipal(identity);
			httpContext.User = claimsPrincipal;

			controller.ControllerContext = new ControllerContext
			{
				HttpContext = httpContext
			};

			controller.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(
				httpContext,
				Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>()
			);

			// Act
			var result = controller.Logout();

			// Assert
			var redirectResult = Assert.IsType<RedirectToActionResult>(result);
			Assert.Equal("Index", redirectResult.ActionName);
			Assert.Equal("Home", redirectResult.ControllerName);
			mockAuthService.Verify(x => x.SignOut(), Times.Once);
		}

		[Fact]
		public void ProfileDetails_ReturnsViewWithUserData()
		{
			// Setup user in HttpContext
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, "testuser"),
				new Claim(ClaimTypes.Role, "Admin")
			};
			var identity = new ClaimsIdentity(claims, "TestAuth");
			var claimsPrincipal = new ClaimsPrincipal(identity);
			var httpContext = new DefaultHttpContext { User = claimsPrincipal };

			var controller = new UserController(
				_fixture.ServiceProvider.GetRequiredService<IUserService>(),
				_fixture.ServiceProvider.GetRequiredService<IMapper>(),
				_mockLogger.Object,
				_fixture.ServiceProvider.GetRequiredService<ILogService>(),
				_fixture.ServiceProvider.GetRequiredService<IAuthService>(),
				_fixture.ServiceProvider.GetRequiredService<IRoleService>()
			)
			{
				ControllerContext = new ControllerContext
				{
					HttpContext = httpContext
				}
			};

			// Act
			var result = controller.ProfileDetails();

			// Assert
			var viewResult = Assert.IsType<ViewResult>(result);
			Assert.NotNull(viewResult.Model);
		}
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
			// Setup services
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

			mockUserService.Setup(x => x.GetById(It.IsAny<int>()))
						  .Returns(user);
			mockUserService.Setup(x => x.GetByUserName(It.IsAny<string>()))
						  .Returns(user);
			mockUserService.Setup(x => x.GetByEmail(It.IsAny<string>()))
						  .Returns((UserDto)null);

			var controller = new UserController(
				mockUserService.Object,
				_fixture.ServiceProvider.GetRequiredService<IMapper>(),
				_mockLogger.Object,
				_fixture.ServiceProvider.GetRequiredService<ILogService>(),
				mockAuthService.Object,
				_fixture.ServiceProvider.GetRequiredService<IRoleService>()
			);

			controller.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(
				new DefaultHttpContext(),
				Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>()
			);

			var userDto = new UserDto
			{
				Id = 1,
				Username = "updated",
				Email = "updated@test.com",
				FirstName = "Updated",
				LastName = "User",
				Role = 1
			};

			// Act
			var result = controller.ProfileEdit(1, userDto);

			// Assert
			var partialViewResult = Assert.IsType<PartialViewResult>(result);
			Assert.Equal("_ProfileDetailsPartial", partialViewResult.ViewName);
			mockUserService.Verify(x => x.Update(It.IsAny<int>(), It.IsAny<UserDto>()), Times.Once);
		}




	}
}