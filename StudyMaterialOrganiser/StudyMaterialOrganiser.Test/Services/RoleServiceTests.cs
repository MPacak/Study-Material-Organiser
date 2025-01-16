using BL.Services;
using BL.Utilities;
using Moq;

public class RoleServiceTests
{
    private readonly Mock<IRoleApprovalStrategy> _mockRoleApprovalStrategy;
    private readonly RoleService _roleService;

    public RoleServiceTests()
    {
        _mockRoleApprovalStrategy = new Mock<IRoleApprovalStrategy>();
        _roleService = new RoleService(_mockRoleApprovalStrategy.Object);
    }

    [Fact]
    public void CanApproveRole_ShouldReturnTrue_WhenApprovalAllowed()
    {
        _mockRoleApprovalStrategy.Setup(r => r.CanApprove(It.IsAny<int?>())).Returns(true);

        var result = _roleService.CanApproveRole(1);

        Assert.True(result);
    }

    [Fact]
    public void GetApprovedRole_ShouldReturnApprovedRole()
    {
        var result = _roleService.GetApprovedRole(1);

        Assert.Equal(1, result);
    }
}
