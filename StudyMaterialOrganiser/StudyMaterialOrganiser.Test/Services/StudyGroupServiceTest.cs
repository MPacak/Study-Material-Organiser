using AutoMapper;
using BL.Models;
using BL.Services;
using DAL.IRepositories;
using DAL.Models;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

public class StudyGroupServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly StudyGroupService _studyGroupService;

        public StudyGroupServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _studyGroupService = new StudyGroupService(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Fact]
        public void Add_ShouldCallRepositoryAddAndSave()
        {
            
            var studyGroupDto = new StudyGroupDto { Id = 1, Name = "Group A", TagId = 10 };
            var studyGroup = new StudyGroup { Id = 1, Name = "Group A", TagId = 10 };

            _mockMapper.Setup(m => m.Map<StudyGroup>(studyGroupDto)).Returns(studyGroup);

            
            _studyGroupService.Add(studyGroupDto);

            
            _mockUnitOfWork.Verify(u => u.StudyGroup.Add(It.Is<StudyGroup>(g => g.Name == "Group A" && g.TagId == 10)), Times.Once);
            _mockUnitOfWork.Verify(u => u.Save(), Times.Once);
        }

        [Fact]
        public void GetAll_ShouldReturnMappedDtos()
        {
            
            var studyGroups = new List<StudyGroup>
            {
                new StudyGroup { Id = 1, Name = "Group A", TagId = 10 },
                new StudyGroup { Id = 2, Name = "Group B", TagId = 20 }
            };

            var studyGroupDtos = new List<StudyGroupDto>
            {
                new StudyGroupDto { Id = 1, Name = "Group A", TagId = 10 },
                new StudyGroupDto { Id = 2, Name = "Group B", TagId = 20 }
            };

            _mockUnitOfWork.Setup(u => u.StudyGroup.GetAll(It.IsAny<Expression<Func<StudyGroup, bool>>>(), "Tag"))
                .Returns(studyGroups);
            _mockMapper.Setup(m => m.Map<IEnumerable<StudyGroupDto>>(studyGroups))
                .Returns(studyGroupDtos);

            
            var result = _studyGroupService.GetAll();

            
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, g => g.Name == "Group A");
            Assert.Contains(result, g => g.Name == "Group B");
        }

        [Fact]
        public void GetGroupById_ShouldReturnMappedDto_WhenGroupExists()
        {
            
            var studyGroup = new StudyGroup { Id = 1, Name = "Group A", TagId = 10 };
            var studyGroupDto = new StudyGroupDto { Id = 1, Name = "Group A", TagId = 10 };

            _mockUnitOfWork.Setup(u => u.StudyGroup.GetFirstOrDefault(It.IsAny<Expression<Func<StudyGroup, bool>>>(), "Tag"))
                .Returns(studyGroup);
            _mockMapper.Setup(m => m.Map<StudyGroupDto>(studyGroup)).Returns(studyGroupDto);

            
            var result = _studyGroupService.GetGroupById(1);

            
            Assert.NotNull(result);
            Assert.Equal("Group A", result.Name);
            Assert.Equal(10, result.TagId);
        }

        [Fact]
        public void GetGroupById_ShouldReturnNull_WhenGroupDoesNotExist()
        {
            
            _mockUnitOfWork.Setup(u => u.StudyGroup.GetFirstOrDefault(It.IsAny<Expression<Func<StudyGroup, bool>>>(), "Tag"))
                .Returns<StudyGroup>(null);

            
            var result = _studyGroupService.GetGroupById(1);

            
            Assert.Null(result);
        }

        [Fact]
        public void Remove_ShouldCallRepositoryRemoveAndSave_WhenGroupExists()
        {
            
            var studyGroup = new StudyGroup { Id = 1, Name = "Group A", TagId = 10 };

            _mockUnitOfWork.Setup(u => u.StudyGroup.GetFirstOrDefault(It.IsAny<Expression<Func<StudyGroup, bool>>>(), null))
                .Returns(studyGroup);

            
            _studyGroupService.Remove(1);

            
            _mockUnitOfWork.Verify(u => u.Save(), Times.Once);
        }

        [Fact]
        public void Remove_ShouldNotCallSave_WhenGroupDoesNotExist()
        {
            
            _mockUnitOfWork.Setup(u => u.StudyGroup.GetFirstOrDefault(It.IsAny<Expression<Func<StudyGroup, bool>>>(), null))
                .Returns<StudyGroup>(null);

            
            _studyGroupService.Remove(1);

            
            _mockUnitOfWork.Verify(u => u.Save(), Times.Never);
        }

        [Fact]
        public void Update_ShouldModifyGroupAndSave_WhenGroupExists()
        {
            
            var studyGroup = new StudyGroup { Id = 1, Name = "Group A", TagId = 10 };
            var updatedDto = new StudyGroupDto { Id = 1, Name = "Updated Group", TagId = 20 };

            _mockUnitOfWork.Setup(u => u.StudyGroup.GetFirstOrDefault(It.IsAny<Expression<Func<StudyGroup, bool>>>(), null))
                .Returns(studyGroup);

            
            _studyGroupService.Update(1, updatedDto);

            
            Assert.Equal("Updated Group", studyGroup.Name);
            Assert.Equal(20, studyGroup.TagId);
            _mockUnitOfWork.Verify(u => u.Save(), Times.Once);
        }
    }

