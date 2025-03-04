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


            _mockFileHandler = new Mock<BaseFileHandler>(Mock.Of<IConfiguration>());


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
        public void Get_Edit_ReturnsViewWithMaterialAndTags()
        {
           
            var dbContext = _fixture.DbContext;
            var material = new Material
            {
                Name = "Edit Material",
                Description = "Edit Description",
                Link = "link",
                FilePath = "testfile.pdf",
                FolderTypeId = 4
            };

            dbContext.Materials.Add(material);
            dbContext.SaveChanges();

           
            var result = _controller.Edit(material.Idmaterial);

          
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<MaterialVM>(viewResult.Model);

            Assert.NotNull(model.AvailableTags);
            Assert.Equal(material.Name, model.Name);
        }
        [Fact]
        public async Task Post_Edit_UpdatesMaterial()
        {
           
            var dbContext = _fixture.DbContext;
            var material = new DAL.Models.Material
            {
                Name = "Old Material",
                Description = "Old Description",
                Link = "link",
                FilePath = "oldfile.pdf",
                FolderTypeId = 4
            };

            dbContext.Materials.Add(material);
            dbContext.SaveChanges();

            var updatedMaterial = new MaterialVM
            {
                Idmaterial = material.Idmaterial,
                Name = "Updated Material",
                Description = "Updated Description",
                FolderTypeId = 3,
                AvailableTags = new List<TagVM>()
            };

          
            var result = await _controller.Edit(material.Idmaterial, updatedMaterial);

      
            var viewResult = Assert.IsType<ViewResult>(result);
            var dbMaterial = dbContext.Materials.FirstOrDefault(m => m.Idmaterial == material.Idmaterial);

            Assert.NotNull(dbMaterial);
            Assert.Equal("Updated Material", dbMaterial.Name);
            Assert.Equal("Updated Description", dbMaterial.Description);
        }
        [Fact]
        public async Task Post_Edit_FailsWhenMaterialNameAlreadyExists()
        {

            var dbContext = _fixture.DbContext;

            var existingMaterialWithDuplicateName = new Material
            {
                Name = "Duplicate Material",
                Description = "Existing material with duplicate name",
                Link = "link1",
                FilePath = "existingfile1.pdf",
                FolderTypeId = 1
            };
            dbContext.Materials.Add(existingMaterialWithDuplicateName);
           
            var materialToUpdate = new Material
            {
                Name = "Original Material",
                Description = "Original Description",
                Link = "link2",
                FilePath = "originalfile.pdf",
                FolderTypeId = 2
            };
            dbContext.Materials.Add(materialToUpdate);

            dbContext.SaveChanges();
            
            var updatedMaterialVM = new MaterialVM
            {
                Idmaterial = materialToUpdate.Idmaterial,
                Name = "Duplicate Material", 
                Description = "Updated Description",
                FolderTypeId = 3,
                AvailableTags = new List<TagVM>()
            };
          
            var result = await _controller.Edit(materialToUpdate.Idmaterial, updatedMaterialVM);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.True(viewResult.ViewName == null || viewResult.ViewName == "Edit",
             $"Expected view name to be 'Edit' or null, but got '{viewResult.ViewName}'");

            var errors = _controller.ModelState["Name"]?.Errors.Select(e => e.ErrorMessage).ToList();
            Assert.NotNull(errors);
            Assert.Contains("A material with this name already exists.", errors);

            var dbMaterial = dbContext.Materials.FirstOrDefault(m => m.Idmaterial == materialToUpdate.Idmaterial);
            Assert.NotNull(dbMaterial);
            Assert.Equal("Original Material", dbMaterial.Name);
            Assert.Equal("Original Description", dbMaterial.Description);
        }

    }
}
