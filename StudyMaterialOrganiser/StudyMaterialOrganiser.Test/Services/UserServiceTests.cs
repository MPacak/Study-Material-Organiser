using System.Linq.Expressions;
using Moq;
using Xunit;
using DAL.IRepositories;
using BL.Services;
using DAL.Models;
using BL.Models;
using AutoMapper;
using BL.IServices;

public class UserServiceTests
{
	private readonly Mock<IUnitOfWork> _mockUnitOfWork;
	private readonly Mock<IAuthService> _mockAuthService;
	private readonly Mock<IMapper> _mockMapper;
	private readonly UserService _userService;

	public UserServiceTests()
	{
		_mockUnitOfWork = new Mock<IUnitOfWork>();
		_mockAuthService = new Mock<IAuthService>();
		_mockMapper = new Mock<IMapper>();

		_userService = new UserService(
			_mockUnitOfWork.Object,
			_mockMapper.Object,
			_mockAuthService.Object
		);
	}

	[Fact]
	public void Create_ShouldAddUserAndSave()
	{
		// Arrange
		var userRegistrationDto = new UserRegistrationDto
		{
			UserName = "test",
			Email = "test@test.com",
			Password = "password",
			FirstName = "Test",
			LastName = "User"
		};

		var user = new User
		{
			Id = 1,
			Username = "test",
			Email = "test@test.com",
			FirstName = "Test",
			LastName = "User",
			PwdHash = "hash",
			PwdSalt = "salt",
			SecurityToken = "token",
			Role = 1,
			IsDeleted = false
		};

		_mockUnitOfWork.Setup(u => u.User.GetFirstOrDefault(
			It.IsAny<Expression<Func<User, bool>>>(),
			It.IsAny<string>()))
			.Returns((User)null);

		_mockMapper.Setup(m => m.Map<User>(It.IsAny<UserRegistrationDto>()))
			.Returns(user);

		_mockMapper.Setup(m => m.Map<UserDto>(It.IsAny<User>()))
			.Returns(new UserDto
			{
				Id = 1,
				Username = "test",
				Email = "test@test.com",
				FirstName = "Test",
				LastName = "User",
				Role = 1
			});

		// Act
		var result = _userService.Create(userRegistrationDto);

		// Assert
		Assert.NotNull(result);
		Assert.Equal("test", result.Username);
		_mockUnitOfWork.Verify(u => u.User.Add(It.IsAny<User>()), Times.Once);
		_mockUnitOfWork.Verify(u => u.Save(), Times.Once);
	}

	[Fact]
	public void Delete_ShouldMarkUserAsDeletedAndSave()
	{
		// Arrange
		var user = new User
		{
			Id = 1,
			IsDeleted = false,
			Username = "test",
			Email = "test@test.com",
			FirstName = "Test",
			LastName = "User",
			PwdHash = "hash",
			PwdSalt = "salt",
			SecurityToken = "token",
			Role = 1
		};

		_mockUnitOfWork.Setup(u => u.User.GetFirstOrDefault(
			It.IsAny<Expression<Func<User, bool>>>(),
			It.IsAny<string>()))
			.Returns(user);

		// Act
		_userService.Delete(1);

		// Assert
		Assert.True(user.IsDeleted);
		_mockUnitOfWork.Verify(u => u.Save(), Times.Once);
	}
}