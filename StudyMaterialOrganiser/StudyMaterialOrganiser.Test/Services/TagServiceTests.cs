using System.Linq.Expressions;
using Moq;
using Xunit;
using DAL.IRepositories;
using BL.Services;
using DAL.Models;
using BL.Models;
using Microsoft.Extensions.Configuration;
using AutoMapper;

public class TagServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IConfiguration> _mockConfiguration; 
    private readonly TagService _tagService;

    public TagServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _mockConfiguration = new Mock<IConfiguration>(); 

        _tagService = new TagService(_mockUnitOfWork.Object, _mockConfiguration.Object, _mockMapper.Object);
    }

    [Fact]
    public void GetById_ShouldReturnTag_WhenTagExists()
    {
        
        var tag = new Tag { Idtag = 1, TagName = "Test Tag" };

        _mockUnitOfWork.Setup(u => u.Tag.GetFirstOrDefault(It.IsAny<Expression<Func<Tag, bool>>>(), It.Is<string>(s => s == null)))
            .Returns(tag);

        _mockMapper.Setup(m => m.Map<TagDto>(tag)).Returns(new TagDto { Id = 1, Name = "Test Tag" });

        
        var result = _tagService.GetById(1);

        
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Tag", result.Name);
    }

    [Fact]
    public void Create_ShouldAddTagAndSave()
    {
        
        var tagDto = new TagDto { Name = "New Tag" };
        var tag = new Tag { TagName = "New Tag" };

        _mockUnitOfWork.Setup(u => u.Tag.GetFirstOrDefault(It.IsAny<Expression<Func<Tag, bool>>>(), It.Is<string>(s => s == null)))
            .Returns((Tag)null);

        _mockMapper.Setup(m => m.Map<Tag>(tagDto)).Returns(tag);

        
        _tagService.Create(tagDto);

        
        _mockUnitOfWork.Verify(u => u.Tag.Add(It.IsAny<Tag>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.Save(), Times.Once);
    }
}
