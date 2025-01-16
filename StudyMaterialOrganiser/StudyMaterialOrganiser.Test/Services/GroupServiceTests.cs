using Xunit;
using Moq;
using BL.Services;
using BL.Models;
using DAL.IRepositories;
using DAL.Models;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BL.IServices;

public class GroupServiceTests
{
	private readonly Mock<IUnitOfWork> _mockUnitOfWork;
	private readonly Mock<IMapper> _mockMapper;
	private readonly Mock<ILogService> _mockLogService;
	private readonly GroupService _groupService;

	public GroupServiceTests()
	{
		_mockUnitOfWork = new Mock<IUnitOfWork>();
		_mockMapper = new Mock<IMapper>();
		_mockLogService = new Mock<ILogService>();

		_groupService = new GroupService(
			_mockUnitOfWork.Object,
			null, 
			_mockMapper.Object,
			_mockLogService.Object
		);
	}
	[Fact]
	public void GetAll_ShouldReturnAllGroups()
	{
		// Arrange
		var groups = new List<StudyGroup>
	{
		new StudyGroup { Id = 1, Name = "Group A", Tag = new Tag { TagName = "Tag1" } },
		new StudyGroup { Id = 2, Name = "Group B", Tag = new Tag { TagName = "Tag2" } }
	};

		// Mock repository to return groups
		_mockUnitOfWork.Setup(uow => uow.Group.GetAll(null, "Tag"))
			.Returns(groups);

		// Mock mapper to return GroupDto with Skills and Users populated
		_mockMapper.Setup(mapper => mapper.Map<ICollection<GroupDto>>(groups))
			.Returns(new List<GroupDto>
			{
			new GroupDto
			{
				Id = 1,
				Name = "Group A",
				TagName = "Tag1",
				Skills = new List<string> { "Skill1", "Skill2" },
				Users = new List<string> { "User1", "User2" }
			},
			new GroupDto
			{
				Id = 2,
				Name = "Group B",
				TagName = "Tag2",
				Skills = new List<string> { "Skill3", "Skill4" },
				Users = new List<string> { "User3", "User4" }
			}
			});

		// Act
		var result = _groupService.GetAll();

		// Assert
		Assert.NotNull(result); // Ensure result is not null
		Assert.Equal(2, result.Count); // Check the number of groups
		Assert.Contains(result, g => g.Name == "Group A" && g.TagName == "Tag1");
		Assert.Contains(result, g => g.Name == "Group B" && g.TagName == "Tag2");
		_mockLogService.Verify(log => log.Log("info", "All Groups were Fetched"), Times.Once);
	}

	[Fact]
	public void GetById_ShouldReturnCorrectGroup()
	{
		// Arrange
		var group = new StudyGroup { Id = 1, Name = "Group A" };
		_mockUnitOfWork.Setup(uow => uow.Group.GetFirstOrDefault(g => g.Id == 1, null))
			.Returns(group);
		_mockMapper.Setup(mapper => mapper.Map<GroupDto>(group))
			.Returns(new GroupDto { Id = 1, Name = "Group A" });

		// Act
		var result = _groupService.GetById(1);

		// Assert
		Assert.NotNull(result);
		Assert.Equal("Group A", result.Name);
		_mockLogService.Verify(log => log.Log("info", "Group with id 1 was Fetched"), Times.Once);
	}
	[Fact]
	public void Create_ShouldAddNewGroup()
	{
		// Arrange
		var groupDto = new GroupDto { Name = "New Group" };
		var group = new StudyGroup { Name = "New Group" };

		_mockUnitOfWork.Setup(uow => uow.Group.GetFirstOrDefault(g => g.Name == groupDto.Name, null))
			.Returns((StudyGroup)null); // No group exists
		_mockMapper.Setup(mapper => mapper.Map<StudyGroup>(groupDto)).Returns(group);
		_mockMapper.Setup(mapper => mapper.Map<GroupDto>(group)).Returns(groupDto);

		// Act
		var result = _groupService.Create(groupDto);

		// Assert
		Assert.NotNull(result);
		Assert.Equal("New Group", result.Name);
		_mockUnitOfWork.Verify(uow => uow.Group.Add(It.Is<StudyGroup>(g => g.Name == "New Group")), Times.Once);
		_mockUnitOfWork.Verify(uow => uow.Save(), Times.Once);
		_mockLogService.Verify(log => log.Log("info", "Group with name New Group was Created"), Times.Once);
	}
	[Fact]
	public void Update_ShouldUpdateExistingGroup()
	{
		// Arrange
		var existingGroup = new StudyGroup { Id = 1, Name = "Old Group" };
		var updatedGroupDto = new GroupDto { Name = "Updated Group" };

		_mockUnitOfWork.Setup(uow => uow.Group.GetFirstOrDefault(g => g.Id == 1, "Tag.TagName"))
			.Returns(existingGroup);
		_mockMapper.Setup(mapper => mapper.Map<GroupDto>(existingGroup))
			.Returns(updatedGroupDto);

		// Act
		var result = _groupService.Update(1, updatedGroupDto);

		// Assert
		Assert.NotNull(result);
		Assert.Equal("Updated Group", result.Name);
		_mockUnitOfWork.Verify(uow => uow.Save(), Times.Once);
		_mockLogService.Verify(log => log.Log("info", "Group with name Updated Group was Updated"), Times.Once);
	}
	[Fact]
	public void Delete_ShouldRemoveGroup()
	{
		// Arrange
		var group = new StudyGroup { Id = 1, Name = "Group To Delete" };
		_mockUnitOfWork.Setup(uow => uow.Group.GetFirstOrDefault(g => g.Id == 1, null))
			.Returns(group);
		_mockMapper.Setup(mapper => mapper.Map<GroupDto>(group))
			.Returns(new GroupDto { Id = 1, Name = "Group To Delete" });

		// Act
		var result = _groupService.Delete(1);

		// Assert
		Assert.NotNull(result);
		Assert.Equal("Group To Delete", result.Name);
		_mockUnitOfWork.Verify(uow => uow.Group.Delete(group), Times.Once);
		_mockUnitOfWork.Verify(uow => uow.Save(), Times.Once);
		_mockLogService.Verify(log => log.Log("info", "Group with name Group To Delete was Deleted"), Times.Once);
	}
	[Fact]
	public void GetCount_ShouldReturnCorrectCount()
	{
		// Arrange
		_mockUnitOfWork.Setup(uow => uow.Group.GetAll(null, null)) // Pass null explicitly
			.Returns(new List<StudyGroup> { new StudyGroup(), new StudyGroup() });

		// Act
		var count = _groupService.GetCount();

		// Assert
		Assert.Equal(2, count);
	}

	// Tests will go here
}