using System.Linq.Expressions;
using Moq;
using Xunit;
using DAL.IRepositories;
using BL.Services;
using DAL.Models;
using BL.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using BL.IServices;

public class AuthServiceTests
{
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IPasswordService> _mockPasswordService;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockPasswordService = new Mock<IPasswordService>();
        _mockConfiguration = new Mock<IConfiguration>();

        
        _mockConfiguration.Setup(c => c["Jwt:Key"]).Returns("MockJwtKey");

        _authService = new AuthService(
            _mockHttpContextAccessor.Object,
            _mockUnitOfWork.Object,
            _mockPasswordService.Object,
            _mockConfiguration.Object
        );
    }

    [Fact]
    public void Authenticate_ShouldReturnUser_WhenValidCredentials()
    {
        
        var user = new User { Username = "testUser", PwdHash = "hashedPwd", PwdSalt = "salt" };
        _mockUnitOfWork.Setup(u => u.User.GetFirstOrDefault(It.IsAny<Expression<Func<User, bool>>>(), null))
            .Returns(user);

        _mockPasswordService.Setup(ps => ps.VerifyPassword("password", "hashedPwd", "salt")).Returns(true);

        
        var result = _authService.Authenticate("testUser", "password");

        
        Assert.NotNull(result);
        Assert.Equal("testUser", result.Username);
    }

    [Fact]
    public void GenerateToken_ShouldReturnToken_WhenValidRequest()
    {
        
        var user = new User { Username = "testUser", Role = 1 };
        _mockUnitOfWork.Setup(u => u.User.GetFirstOrDefault(It.IsAny<Expression<Func<User, bool>>>(), null))
            .Returns(user);

        var request = new UserLoginDto { Username = "testUser", Password = "password" };
        _mockPasswordService.Setup(ps => ps.VerifyPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(true);

        
        var token = _authService.GenerateToken(request);

        
        Assert.NotNull(token);
        Assert.IsType<string>(token);
    }
}
