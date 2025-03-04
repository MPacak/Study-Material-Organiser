﻿using AutoMapper;
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
    public class MaterialControllerIntegrationTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;
        private readonly MaterialController _controller;
        private readonly Mock<IWebHostEnvironment> _mockWebHostEnvironment;
        private readonly Mock<BaseFileHandler> _mockFileHandler;

        private readonly AssignTags _assignTags;

        public MaterialControllerIntegrationTests(TestFixture fixture)
        {
            var materialService = fixture.ServiceProvider.GetRequiredService<IMaterialService>();
            var mapper = fixture.ServiceProvider.GetRequiredService<IMapper>();
            var fileHandler = fixture.ServiceProvider.GetRequiredService<BaseFileHandler>();
            var assignTags = fixture.ServiceProvider.GetRequiredService<AssignTags>();
            _fixture = fixture;
            _mockWebHostEnvironment = new Mock<IWebHostEnvironment>();


            _mockFileHandler = new Mock<BaseFileHandler>(Mock.Of<IConfiguration>());


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
        public async Task CannotCreateMaterialWithDuplicateName()
        {
            var dbContext = _fixture.DbContext;
            var materialService = _fixture.ServiceProvider.GetRequiredService<IMaterialService>();


            if (!dbContext.Tags.Any())
            {
                dbContext.Tags.Add(new Tag { TagName = "TestTag" });
                await dbContext.SaveChangesAsync();
            }

            var tags = await dbContext.Tags.ToListAsync();
            var tagId = tags.First().Idtag;

            var existingMaterial = new Material
            {
                Name = "Duplicate Material",
                Description = "This is an existing material",
                FilePath = "existing-file-path.pdf",
                FolderTypeId = 1,
                Link = "http://localhost/Material/Access/1"
            };

            dbContext.Materials.Add(existingMaterial);
            await dbContext.SaveChangesAsync();

            var fileContent = Encoding.UTF8.GetBytes("Dummy file content");
            var materialVM = new MaterialVM
            {
                Name = "Duplicate Material", 
                Description = "This is a duplicate",
                File = new FormFile(
                    new MemoryStream(fileContent),
                    0,
                    fileContent.Length,
                    "Data",
                    "testfile.pdf"
                ),
                TagIds = new List<int> { tagId }
            };

            var result = await _controller.Create(materialVM);


            Assert.False(_controller.ModelState.IsValid, "ModelState should be invalid for duplicate material name.");

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(null, viewResult.ViewName); 

            var model = Assert.IsType<MaterialVM>(viewResult.Model);
            Assert.Equal("Duplicate Material", model.Name);

            var errors = _controller.ModelState["Name"]?.Errors.Select(e => e.ErrorMessage).ToList();
            Assert.NotNull(errors);
            Assert.Contains("A material with this name already exists.", errors);

            dbContext.Materials.RemoveRange(dbContext.Materials);
            dbContext.Tags.RemoveRange(dbContext.Tags);
            await dbContext.SaveChangesAsync();
        }

        [Fact]
        public async Task CanCreateMaterialViaController()
        {
            var dbContext = _fixture.DbContext;
            var materialService = _fixture.ServiceProvider.GetRequiredService<IMaterialService>();

            var serviceType = materialService.GetType();
            Console.WriteLine($"Material service type: {serviceType.FullName}");

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
                TagIds = new List<int> { tagId }

            };


            var result = await _controller.Create(materialVM);


            var viewResult = Assert.IsType<ViewResult>(result);

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

            var createdMaterialInDb = materialService.GetMaterialByName("Integration Material");
            Assert.NotNull(createdMaterialInDb);
            Assert.Equal("Created in integration test", createdMaterialInDb.Description);
            var createdMaterial = await dbContext.Materials
          .Include(m => m.MaterialTags)
          .FirstOrDefaultAsync(m => m.Name == "Integration Material");

            Assert.NotNull(createdMaterial);
            Assert.Equal("Created in integration test", createdMaterial.Description);
            Assert.NotNull(createdMaterial.FilePath);
            Assert.Contains(createdMaterial.MaterialTags, mt => mt.TagId == tagId);


            dbContext.MaterialTags.RemoveRange(dbContext.MaterialTags);
            dbContext.Materials.RemoveRange(dbContext.Materials);
            dbContext.Tags.RemoveRange(dbContext.Tags);
            await dbContext.SaveChangesAsync();

        }
        [Fact]
        public void Get_Create_ReturnsViewWithAvailableTags()
        {
            var dbContext = _fixture.DbContext;


            if (!dbContext.Tags.Any())
            {
                dbContext.Tags.AddRange(new List<Tag>
        {
            new Tag { TagName = "Tag1" },
            new Tag { TagName = "Tag2" },
            new Tag { TagName = "Tag3" }
        });
                dbContext.SaveChanges();
            }

            var result = _controller.Create();


            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<MaterialVM>(viewResult.Model);


            Assert.NotNull(model.AvailableTags);
            Assert.True(model.AvailableTags.Any(), "AvailableTags should not be empty.");
        }


    }
}