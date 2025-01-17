using BL.Services;

public class MaterialAccessServiceTests
{
    private readonly MaterialAccessService _materialAccessService;

    public MaterialAccessServiceTests()
    {
        _materialAccessService = new MaterialAccessService();
    }

    [Fact]
    public void CanAccessMaterial_ShouldAlwaysReturnTrue()
    {
        var result = _materialAccessService.CanAccessMaterial(1, 1, "view");

        Assert.True(result);
    }

    [Fact]
    public void GetPermission_ShouldReturnViewPermission()
    {
        var result = _materialAccessService.GetPermission(1, 1);

        Assert.Equal("view", result);
    }
}
