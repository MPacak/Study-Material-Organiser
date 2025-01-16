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

namespace StudyMaterialOrganiser.Test
{
    public class MaterialControllerIntegrationTests :IClassFixture<TestFixture>
    {
        private readonly MaterialController _controller;
        private readonly TestFixture _fixture;

        public MaterialControllerIntegrationTests(TestFixture fixture)
        {
            var materialService = fixture.ServiceProvider.GetRequiredService<IMaterialService>();
            var mapper = fixture.ServiceProvider.GetRequiredService<IMapper>();
            var fileHandler = fixture.ServiceProvider.GetRequiredService<BaseFileHandler>();
            var assignTags = fixture.ServiceProvider.GetRequiredService<AssignTags>();
            _fixture = fixture;

            _controller = new MaterialController(
                materialService,
                mapper,
                Mock.Of<IWebHostEnvironment>(),
                assignTags,
                fileHandler,
                fixture.ServiceProvider.GetRequiredService<IUserService>(),
                fixture.ServiceProvider.GetRequiredService<IMaterialAccessService>(),
                fixture.ServiceProvider.GetRequiredService<IMaterialFactory>()
            );
        }

        [Fact]
        public async Task CanCreateMaterialViaController()
        {
            
            var materialVM = new MaterialVM
            {
                Name = "Integration Material",
                Description = "Created in integration test"
            };

            
            var result = await _controller.Create(materialVM);

            
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Confirmation", viewResult.ViewName);

            // Verify material exists in the database
            var dbContext = _fixture.DbContext;
            var createdMaterial = dbContext.Materials.FirstOrDefault(m => m.Name == "Integration Material");
            Assert.NotNull(createdMaterial);
        }
    }
}
