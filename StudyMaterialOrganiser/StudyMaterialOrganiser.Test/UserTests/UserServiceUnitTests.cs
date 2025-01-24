using System.Linq.Expressions;
using AutoMapper;
using BL.IServices;
using BL.Models;
using BL.Services;
using DAL.IRepositories;
using DAL.Models;
using Moq;

public class UserServiceUnitTests
{
	private readonly Mock<IUnitOfWork> _mockUnitOfWork;
	private readonly Mock<IMapper> _mockMapper;
	private readonly Mock<IAuthService> _mockAuthService;
	private readonly UserService _userService;

	public UserServiceUnitTests()
	{
		_mockUnitOfWork = new Mock<IUnitOfWork>();
		_mockMapper = new Mock<IMapper>();
		_mockAuthService = new Mock<IAuthService>();
		_userService = new UserService(_mockUnitOfWork.Object, _mockMapper.Object, _mockAuthService.Object);
	}

	[Fact]
	public void Create_WhenFirstUser_AssignsAdminRole()
	{
		// Arrange
		var registration = new UserRegistrationDto
		{
			UserName = "firstadmin",
			Email = "admin@test.com",
			Password = "password"
		};

		var mockUserRepo = new Mock<IUserRepository>();
		mockUserRepo.Setup(r => r.GetFirstOrDefault(
			It.IsAny<Expression<Func<User, bool>>>(),
			null as string))
			.Returns((User)null);

		_mockUnitOfWork.Setup(u => u.User).Returns(mockUserRepo.Object);

		// Act
		var result = _userService.Create(registration);

		// Assert
		mockUserRepo.Verify(r => r.Add(It.Is<User>(u => u.Role == 2)), Times.Once);
	}

	    [Fact]
    public void Update_WithDeletedUser_ThrowsException()
    {
        // Arrange
        var userDto = new UserDto 
        { 
            Id = 1, 
            Username = "test",
            FirstName = "Test",
            LastName = "User",
            Email = "test@test.com"
        };
        
        var mockUserRepo = new Mock<IUserRepository>();
        var deletedUser = new User 
        { 
            Id = 1, 
            IsDeleted = true,
            Username = "test",
            FirstName = "Test",
            LastName = "User",
            Email = "test@test.com"
        };
        
        mockUserRepo.Setup(r => r.GetFirstOrDefault(
            It.IsAny<Expression<Func<User, bool>>>(),
            null as string))
            .Returns(deletedUser);

        _mockUnitOfWork.Setup(u => u.User).Returns(mockUserRepo.Object);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(
            () => _userService.Update(1, userDto));
            
        Assert.Equal("Cannot update deleted user", exception.Message);
    }

	[Fact]
	public void Delete_SetsDeletedFlagAndTimestamp()
	{
		// Arrange
		var user = new User { Id = 1, IsDeleted = false };
		var mockUserRepo = new Mock<IUserRepository>();

		mockUserRepo.Setup(r => r.GetFirstOrDefault(
			It.IsAny<Expression<Func<User, bool>>>(),
			null as string))
			.Returns(user);

		_mockUnitOfWork.Setup(u => u.User).Returns(mockUserRepo.Object);

		// Act
		_userService.Delete(1);

		// Assert
		Assert.True(user.IsDeleted);
		Assert.NotNull(user.DeletedAt);
		_mockUnitOfWork.Verify(u => u.Save(), Times.Once);
	}
}