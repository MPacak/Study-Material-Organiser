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
    public class MaterialDeleteTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;
        private readonly MaterialController _controller;
        private readonly Mock<IWebHostEnvironment> _mockWebHostEnvironment;
        private readonly Mock<BaseFileHandler> _mockFileHandler;

        private readonly AssignTags _assignTags;

        public MaterialDeleteTests(TestFixture fixture)
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
        public async Task CanDeleteMaterialViaController()
        {
            var dbContext = _fixture.DbContext;

            var material = new Material
            {
                Name = "Delete Material",
                Description = "This material will be deleted",
                FilePath = "test-path.pdf",
                Link = "link",
                FolderTypeId = 4
            };

           
            dbContext.Materials.Add(material);
         
           
            await dbContext.SaveChangesAsync();
 
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

        
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Confirmation", viewResult.ViewName);

            var model = Assert.IsType<ConfirmationVM>(viewResult.Model);
            Assert.Equal("Material was successfully deleted.", model.Message);
            Assert.Equal("List", model.ActionName);
            Assert.Equal("Material", model.ControllerName);
            Assert.Equal(3, model.RedirectSeconds);

       
            var deletedMaterial = await dbContext.Materials
                .FirstOrDefaultAsync(m => m.Idmaterial == material.Idmaterial);
            Assert.Null(deletedMaterial);
        }
        [Fact]
        public void Get_Delete_ReturnsViewWithMaterial()
        {
           
            var dbContext = _fixture.DbContext;
            var material = new Material
            {
                Name = "Deletion Material",
                Description = "Deletion Description",
                Link = "link",
                FilePath = "testfile.pdf",
                FolderTypeId = 4
            };

            dbContext.Materials.Add(material);
            dbContext.SaveChanges();

     
            var result = _controller.Delete(material.Idmaterial);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<MaterialVM>(viewResult.Model);

            Assert.Equal(material.Name, model.Name);
        }

    }
}

