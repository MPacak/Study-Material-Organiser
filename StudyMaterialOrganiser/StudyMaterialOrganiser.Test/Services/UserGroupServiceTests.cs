using System.Linq.Expressions;
using Moq;
using Xunit;
using DAL.IRepositories;
using BL.Services;
using DAL.Models;
using BL.Models;
using AutoMapper;

public class UserGroupServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly UserGroupService _userGroupService;

    public UserGroupServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();

        _userGroupService = new UserGroupService(_mockUnitOfWork.Object, _mockMapper.Object);
    }

    [Fact]
    public void Create_ShouldAddUserGroupAndSave()
    {
        
        var userGroupDto = new UserGroupDto { UserId = 1, GroupId = 1 };
        var userGroup = new UserGroup { UserId = 1, GroupId = 1 };

        _mockUnitOfWork.Setup(u => u.UserGroup.GetFirstOrDefault(It.IsAny<Expression<Func<UserGroup, bool>>>(), null))
            .Returns((UserGroup)null);

        _mockMapper.Setup(m => m.Map<UserGroup>(It.IsAny<UserGroupDto>())).Returns(userGroup);

        
        var result = _userGroupService.Create(userGroupDto);

        
        Assert.NotNull(result);
        _mockUnitOfWork.Verify(u => u.UserGroup.Add(It.IsAny<UserGroup>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.Save(), Times.Once);
    }


    public void Delete_ShouldRemoveUserGroupAndSave()
    {
        
        var userGroup = new UserGroup { Id = 1 };

        _mockUnitOfWork.Setup(u => u.UserGroup.GetFirstOrDefault(
            It.IsAny<Expression<Func<UserGroup, bool>>>(),
            It.Is<string>(s => s == null)))
            .Returns(userGroup);

        
        var result = _userGroupService.Delete(1);

        
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        _mockUnitOfWork.Verify(u => u.UserGroup.Delete(It.Is<UserGroup>(ug => ug.Id == 1)), Times.Once);
        _mockUnitOfWork.Verify(u => u.Save(), Times.Once);
    }

}
