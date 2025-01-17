using AutoMapper;
using BL.IServices;
using BL.Models;
using BL.Services;
using DAL.IRepositories;
using DAL.Models;
using Moq;
using System.Linq.Expressions;

public class MaterialServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IMaterialTagService> _mockMaterialTagService;
    private readonly MaterialService _materialService;

    public MaterialServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _mockMaterialTagService = new Mock<IMaterialTagService>();

        _materialService = new MaterialService(
            _mockUnitOfWork.Object,
            null,
            _mockMapper.Object,
            _mockMaterialTagService.Object
        );
    }

    [Fact]
    public void Create_ShouldAddMaterialAndSave()
    {
        var materialDto = new MaterialDto { Idmaterial = 1, Name = "Material A" };
        var material = new Material { Idmaterial = 1, Name = "Material A" };

        _mockMapper.Setup(m => m.Map<Material>(materialDto)).Returns(material);

        _materialService.Create(materialDto);

        _mockUnitOfWork.Verify(u => u.Material.Add(It.Is<Material>(m => m.Name == "Material A")), Times.Once);
        _mockUnitOfWork.Verify(u => u.Save(), Times.AtLeastOnce);
    }

    [Fact]
    public void Delete_ShouldRemoveMaterialAndSave()
    {
        var material = new Material
        {
            Idmaterial = 1,
            Name = "Material A",
            MaterialTags = new List<MaterialTag>
            {
                new MaterialTag { MaterialId = 1, TagId = 1 }
            }
        };

        _mockUnitOfWork.Setup(u => u.Material.GetFirstOrDefault(It.IsAny<Expression<Func<Material, bool>>>(), null))
     .Returns(new Material());

        _materialService.Delete(new MaterialDto { Idmaterial = 1 });

        _mockUnitOfWork.Verify(u => u.MaterialTag.Delete(It.IsAny<MaterialTag>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.Material.Delete(material), Times.Once);
        _mockUnitOfWork.Verify(u => u.Save(), Times.Exactly(2));
    }
}
