using System.Linq.Expressions; 
using Moq;
using Xunit;
using DAL.IRepositories;
using BL.Services;
using DAL.Models;

public class UserValidationServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly UserValidationService _userValidationService;

    public UserValidationServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _userValidationService = new UserValidationService(_mockUnitOfWork.Object);
    }

    [Fact]
    public void IsUsernameAvailable_ShouldReturnTrue_WhenUsernameDoesNotExist()
    {
        
        _mockUnitOfWork.Setup(u => u.User.GetFirstOrDefault(It.IsAny<Expression<Func<User, bool>>>(), null))
            .Returns((User)null);

        
        var result = _userValidationService.IsUsernameAvailable("test");

        
        Assert.True(result);
    }

    [Fact]
    public void IsEmailAvailable_ShouldReturnFalse_WhenEmailExists()
    {
        
        var existingUser = new User { Id = 1, Email = "test@test.com" };

        _mockUnitOfWork.Setup(u => u.User.GetFirstOrDefault(It.IsAny<Expression<Func<User, bool>>>(), null))
            .Returns(existingUser);

        
        var result = _userValidationService.IsEmailAvailable("test@test.com");

        
        Assert.False(result);
    }

}
