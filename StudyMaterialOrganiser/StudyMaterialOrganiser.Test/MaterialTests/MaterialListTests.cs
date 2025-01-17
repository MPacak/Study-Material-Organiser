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
    public class MaterialListTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;
        private readonly MaterialController _controller;
        private readonly Mock<IWebHostEnvironment> _mockWebHostEnvironment;
        private readonly Mock<BaseFileHandler> _mockFileHandler;

        private readonly AssignTags _assignTags;

        public MaterialListTests(TestFixture fixture)
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
        public void Get_Details_ReturnsViewWithMaterial()
        {
            // Arrange
            var dbContext = _fixture.DbContext;
            var material = new Material
            {
                Name = "Detail Material",
                Description = "Detail Description",
                Link = "link",
                FilePath = "testfile.pdf",
                FolderTypeId = 3
            };

            dbContext.Materials.Add(material);
            dbContext.SaveChanges();


            var result = _controller.Details(material.Idmaterial);

    
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<MaterialVM>(viewResult.Model);

            Assert.Equal(material.Name, model.Name);
        }
        [Fact]
        public void Get_List_ReturnsViewWithFilteredMaterials()
        {
    
            var dbContext = _fixture.DbContext;
            dbContext.Materials.AddRange(new List<Material>
    {
        new Material { Name = "Test1", Description = "Description1", FilePath = "test-path.pdf",
                Link = "link", FolderTypeId = 1 },
        new Material { Name = "Test2", Description = "Description2", FilePath = "test-path.pdf",
                Link = "link", FolderTypeId = 2 }
    });
            dbContext.SaveChanges();
         

            //testing name filter
            var result = _controller.List("Test", null, null);

           
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<MaterialSearchVM>(viewResult.Model);
            model.Materials.ForEach(m => Console.WriteLine(m.Name));
            Assert.Equal(2, model.Materials.Count);

            Assert.Contains(model.Materials, m => m.Name == "Test1");
         
            Assert.Contains(model.Materials, m => m.Name == "Test2");
            

            //testing filetype filter
            Console.WriteLine("started filter by filetype");
            result = _controller.List(null, 1, null);
            Console.WriteLine("Finished filter by filetype");

            viewResult = Assert.IsType<ViewResult>(result);
            Console.WriteLine(viewResult.Model);
            model = Assert.IsType<MaterialSearchVM>(viewResult.Model);
            Console.WriteLine(model);
            model.Materials.ForEach(m => Console.WriteLine(m.Name));
            Assert.Equal(1, model.Materials.Count);

            Assert.Contains(model.Materials, m => m.Name == "Test1");


        }

    }
}
