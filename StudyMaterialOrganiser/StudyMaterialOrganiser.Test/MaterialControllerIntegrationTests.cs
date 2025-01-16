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

namespace StudyMaterialOrganiser.Test
{
    public class MaterialControllerIntegrationTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;
        private readonly MaterialController _controller;
        private readonly Mock<IWebHostEnvironment> _mockWebHostEnvironment;
        private readonly Mock<BaseFileHandler> _mockFileHandler;
       
        private readonly AssignTags _assignTags;

        public MaterialControllerIntegrationTests(TestFixture fixture)
        {
            _fixture = fixture;
            _mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            
            

            // Create mock for BaseFileHandler
            _mockFileHandler = new Mock<BaseFileHandler>(Mock.Of<IConfiguration>());

            // Setup the base file handler mock
            _mockFileHandler.Setup(x => x.IsValidFile(It.IsAny<string>()))
                .Returns(true);
            _mockFileHandler.Setup(x => x.SaveFile(It.IsAny<IFormFile>(), It.IsAny<string>()))
                .Returns("testfile.pdf");
            _mockFileHandler.Setup(x => x.GetFileTypeId(It.IsAny<IFormFile>()))
                .Returns(1);
            var binaryStoragePath = Path.Combine(Directory.GetCurrentDirectory(), "binary_storage");
            _mockWebHostEnvironment.Setup(x => x.WebRootPath)
                .Returns(Directory.GetCurrentDirectory());

            _mockWebHostEnvironment.Setup(x => x.WebRootPath)
                .Returns("test-path");

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
        public async Task CanCreateMaterialViaController()
        {
            var dbContext = _fixture.DbContext;
            if (!dbContext.Tags.Any())
            {
                dbContext.Tags.Add(new Tag { TagName = "TestTag" });
                await dbContext.SaveChangesAsync();
            }
            var tags = await dbContext.Tags.ToListAsync();
            var tagId = tags.First().Idtag;
            var existingMaterial = await dbContext.Materials
      .FirstOrDefaultAsync(m => m.Name == "Integration Material");
            if (existingMaterial != null)
            {
                dbContext.Materials.Remove(existingMaterial);
                await dbContext.SaveChangesAsync();
            }
            // Arrange
            var fileContent = Encoding.UTF8.GetBytes("Dummy file content");
            var materialVM = new MaterialVM
            {
                Name = "Integration Material",
                Description = "Created in integration test",
                File = new FormFile(
                    new MemoryStream(fileContent),
                    0,
                    fileContent.Length,
                    "Data",
                    "testfile.pdf"
                ),
                TagIds = new List<int> { 1 },
                //AvailableTags = _assignTags.AssignTag()
            };
            Console.WriteLine("Initial ModelState validity: " + _controller.ModelState.IsValid);
            foreach (var modelState in _controller.ModelState)
            {
                Console.WriteLine($"Property: {modelState.Key}, Value: {modelState.Value?.RawValue}");
            }
            // Act
            var result = await _controller.Create(materialVM);
            Console.WriteLine("ModelState validity after result: " + _controller.ModelState.IsValid);
            if (!_controller.ModelState.IsValid)
            {
                foreach (var entry in _controller.ModelState)
                {
                    Console.WriteLine($"Key: {entry.Key}");
                    foreach (var error in entry.Value.Errors)
                    {
                        Console.WriteLine($"  Error: {error.ErrorMessage}");
                        if (error.Exception != null)
                        {
                            Console.WriteLine($"  Exception: {error.Exception.Message}");
                        }
                    }
                }

                Assert.True(false, "ModelState is invalid. See logs for details.");
            }
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Console.WriteLine($"viewresult point + {viewResult}");
            if (viewResult.ViewName != "Confirmation")
            {
                var actualModel = viewResult.Model;
                var actualViewName = viewResult.ViewName ?? "null";
                Assert.True(false, $"Expected ViewName 'Confirmation' but got '{actualViewName}'. Model type: {actualModel?.GetType().Name ?? "null"}");
            }
            Assert.Equal("Confirmation", viewResult.ViewName);

            var model = Assert.IsType<ConfirmationVM>(viewResult.Model);
            Assert.Equal("Material was successfully created.", model.Message);
            Assert.Equal("List", model.ActionName);
            Assert.Equal("Material", model.ControllerName);
            Assert.Equal(3, model.RedirectSeconds);

            var createdMaterial = await dbContext.Materials
          .Include(m => m.MaterialTags) // Include related tags
          .FirstOrDefaultAsync(m => m.Name == "Integration Material");

            Assert.NotNull(createdMaterial);
            Assert.Equal("Created in integration test", createdMaterial.Description);
            Assert.NotNull(createdMaterial.FilePath);
            Assert.Contains(createdMaterial.MaterialTags, mt => mt.TagId == tagId);

          
        }
        [Fact]
        public async Task CanDeleteMaterialViaController()
        {
            // Arrange: Add a material to the database for deletion
            var dbContext = _fixture.DbContext;

            var material = new DAL.Models.Material
            {
                Name = "Test Material for Deletion",
                Description = "This material will be deleted",
                FilePath = "test-path.pdf",
                Link = "http://localhost/material",
                FolderTypeId = 1
            };

            dbContext.Materials.Add(material);
            await dbContext.SaveChangesAsync();

            // Act: Call the Delete POST method
            var materialVM = new MaterialVM
            {
                Idmaterial = material.Idmaterial,
                Name = material.Name,
                Description = material.Description,
                FilePath = material.FilePath,
                Link = material.Link,
                FolderTypeId = material.FolderTypeId
            };

            var result = _controller.Delete(material.Idmaterial, materialVM);

            // Assert: Check the result
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Confirmation", viewResult.ViewName);

            var model = Assert.IsType<ConfirmationVM>(viewResult.Model);
            Assert.Equal("Material was successfully deleted.", model.Message);
            Assert.Equal("List", model.ActionName);
            Assert.Equal("Material", model.ControllerName);
            Assert.Equal(3, model.RedirectSeconds);

            // Verify the material is removed from the database
            var deletedMaterial = await dbContext.Materials
                .FirstOrDefaultAsync(m => m.Idmaterial == material.Idmaterial);
            Assert.Null(deletedMaterial);
        }

    }
}