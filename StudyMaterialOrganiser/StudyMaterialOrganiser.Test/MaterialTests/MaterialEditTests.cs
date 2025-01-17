using AutoMapper;
using BL.IServices;
using BL.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using StudyMaterialOrganiser.Controllers;
using StudyMaterialOrganiser.Utilities;
using StudyMaterialOrganiser.ViewModels;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using BL.Models;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace StudyMaterialOrganiser.Test.MaterialTests
{
    public class MaterialEditTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;
        private readonly MaterialController _controller;
        private readonly Mock<IWebHostEnvironment> _mockWebHostEnvironment;
        private readonly Mock<BaseFileHandler> _mockFileHandler;

        private readonly AssignTags _assignTags;

        public MaterialEditTests(TestFixture fixture)
        {
            _fixture = fixture;
            _mockWebHostEnvironment = new Mock<IWebHostEnvironment>();



            // Create mock for BaseFileHandler
            _mockFileHandler = new Mock<BaseFileHandler>(Mock.Of<IConfiguration>());


            var realTagService = _fixture.ServiceProvider.GetRequiredService<ITagService>();
            var realMapper = _fixture.ServiceProvider.GetRequiredService<IMapper>();

            // Create the real AssignTags instance with real dependencies
            _assignTags = new AssignTags(realTagService, realMapper);



            _controller = new MaterialController(
                _fixture.ServiceProvider.GetRequiredService<IMaterialService>(),
                _fixture.ServiceProvider.GetRequiredService<IMapper>(),
                _mockWebHostEnvironment.Object,
               _assignTags,
                _mockFileHandler.Object,
                _fixture.ServiceProvider.GetRequiredService<IUserService>(),
                _fixture.ServiceProvider.GetRequiredService<IMaterialAccessService>(),
                _fixture.ServiceProvider.GetRequiredService<IMaterialFactory>()
            );
        }
        [Fact]
        public void Get_Edit_ReturnsViewWithMaterialAndTags()
        {
            // Arrange
            var dbContext = _fixture.DbContext;
            var material = new Material
            {
                Name = "Test Material",
                Description = "Test Description",
                Link = "link",
                FilePath = "testfile.pdf",
                FolderTypeId = 1
            };

            dbContext.Materials.Add(material);
            dbContext.SaveChanges();

            // Act
            var result = _controller.Edit(material.Idmaterial);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<MaterialVM>(viewResult.Model);

            Assert.NotNull(model.AvailableTags);
            Assert.Equal(material.Name, model.Name);
        }
        [Fact]
        public async Task Post_Edit_UpdatesMaterial()
        {
            // Arrange
            var dbContext = _fixture.DbContext;
            var material = new DAL.Models.Material
            {
                Name = "Old Material",
                Description = "Old Description",
                Link = "link",
                FilePath = "oldfile.pdf",
                FolderTypeId = 1
            };

            dbContext.Materials.Add(material);
            dbContext.SaveChanges();

            var updatedMaterial = new MaterialVM
            {
                Idmaterial = material.Idmaterial,
                Name = "Updated Material",
                Description = "Updated Description",
                FolderTypeId = 1,
                AvailableTags = new List<TagVM>()
            };

            // Act
            var result = await _controller.Edit(material.Idmaterial, updatedMaterial);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var dbMaterial = dbContext.Materials.FirstOrDefault(m => m.Idmaterial == material.Idmaterial);

            Assert.NotNull(dbMaterial);
            Assert.Equal("Updated Material", dbMaterial.Name);
            Assert.Equal("Updated Description", dbMaterial.Description);
        }

    }
}
