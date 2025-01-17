using BL.Services;

public class PasswordServiceTests
{
    private readonly PasswordService _passwordService;

    public PasswordServiceTests()
    {
        _passwordService = new PasswordService();
    }

    [Fact]
    public void GenerateSalt_ShouldReturnNonEmptyString()
    {
        var salt = _passwordService.GenerateSalt();

        Assert.NotNull(salt);
        Assert.NotEmpty(salt);
    }

    [Fact]
    public void HashPassword_ShouldReturnHash()
    {
        var salt = _passwordService.GenerateSalt();
        var hash = _passwordService.HashPassword("password", salt);

        Assert.NotNull(hash);
        Assert.NotEqual("password", hash);
    }

    [Fact]
    public void VerifyPassword_ShouldReturnTrue_WhenValid()
    {
        var salt = _passwordService.GenerateSalt();
        var hash = _passwordService.HashPassword("password", salt);

        var result = _passwordService.VerifyPassword("password", hash, salt);

        Assert.True(result);
    }
}
