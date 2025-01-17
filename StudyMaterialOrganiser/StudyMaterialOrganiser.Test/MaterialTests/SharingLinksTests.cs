using Moq;
using Microsoft.AspNetCore.Mvc;
using BL.Models;
using BL.IServices;
using AutoMapper;
using StudyMaterialOrganiser.Controllers;
using StudyMaterialOrganiser.ViewModels;
using Microsoft.AspNetCore.Hosting;
using StudyMaterialOrganiser.Utilities;
using BL.Utilities;
using BL.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using DAL.Models;
using Microsoft.AspNetCore.Http;

namespace StudyMaterialOrganiser.Test.MaterialTests
{
    public class SharingLinksTests : IClassFixture<TestFixture>
    {

        private readonly TestFixture _fixture;
        private readonly MaterialController _controller;

        public SharingLinksTests(TestFixture fixture)
        {
            _fixture = fixture;

            
            var realAssignTags = new AssignTags(
                _fixture.ServiceProvider.GetRequiredService<ITagService>(),
                _fixture.ServiceProvider.GetRequiredService<IMapper>()
            );

            var realMaterialFactory = new MaterialFactory();
            var configuration = _fixture.ServiceProvider.GetRequiredService<IConfiguration>();
            var realFileHandler = new BinaryFileHandler(configuration);

            _controller = new MaterialController(
                _fixture.ServiceProvider.GetRequiredService<IMaterialService>(),
                _fixture.ServiceProvider.GetRequiredService<IMapper>(),
                Mock.Of<IWebHostEnvironment>(), 
                realAssignTags,
                 realFileHandler,
                _fixture.ServiceProvider.GetRequiredService<IUserService>(),
                _fixture.ServiceProvider.GetRequiredService<IMaterialAccessService>(),
                realMaterialFactory
                );
        }

        [Fact]
        public void ShareWithUsers_ShouldReturnViewWithViewModel()
        {
            var materialId = 1;
            var searchTerm = "John";

            var dbContext = _fixture.DbContext;

            if (!dbContext.Materials.Any(m => m.Idmaterial == materialId))
            {
                dbContext.Materials.Add(new Material
                {
                    Idmaterial = materialId,
                    Name = "Share Material",
                    Description = "Share Description",
                    FilePath = "test-path.pdf",
                    Link = "link",
                    FolderTypeId = 1
                });
                dbContext.SaveChanges();
            }

            if (!dbContext.Users.Any())
            {
                dbContext.Users.AddRange(new List<User>
        {
           new User
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Username = "john.doe",
                PwdHash = "new byte[] { 1, 2, 3 }", 
                PwdSalt = "new byte[] { 1, 2, 3 }", 
                SecurityToken = Guid.NewGuid().ToString()
            },
            new User
            {
                Id = 2,
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                Username = "jane.smith",
                PwdHash = "new byte[] { 1, 2, 3 }", 
                PwdSalt = "new byte[] { 1, 2, 3 }", 
                SecurityToken = Guid.NewGuid().ToString()
            }
        });
                dbContext.SaveChanges();
            }
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "https";
            httpContext.Request.Host = new HostString("localhost");

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };


            var result = _controller.ShareWithUsers(materialId, searchTerm);

            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsType<ShareWithUsersViewModel>(viewResult.Model);

            Assert.Equal(materialId, viewModel.MaterialId);
            Assert.Equal($"https://localhost/Material/Access/{materialId}", viewModel.MaterialLink);
            Assert.Equal(searchTerm, viewModel.SearchTerm);
            Assert.NotEmpty(viewModel.Users);

            var firstUser = viewModel.Users.First();
            Assert.Equal(1, firstUser.Id);
            Assert.Equal("John", firstUser.FirstName);
            Assert.Equal("Doe", firstUser.LastName);
            Assert.Equal("john.doe@example.com", firstUser.Email);
            Assert.Equal("view", firstUser.Permission);
        }



        [Fact]
        public void SetPermission_ShouldReturnSuccessJson()
        {
            
            var materialId = 1;
            var userId = 2;
            var permission = "Write";

            dynamic requestData = new System.Dynamic.ExpandoObject();
            requestData.materialId = materialId;
            requestData.userId = userId;
            requestData.permission = permission;


            var result = _controller.SetPermission(requestData);

            var jsonResult = Assert.IsType<JsonResult>(result);
            var response = Newtonsoft.Json.Linq.JObject.FromObject(jsonResult.Value);
            Console.WriteLine(response);
            Assert.False((bool)response["success"]);
            Assert.Equal("Permissions are handled by the proxy", (string)response["message"]);


        }

        [Fact]
        public void SetPermission_ShouldReturnErrorJson_WhenPermissionIsEmpty()
        {
            
            var materialId = 1;
            var userId = 2;
            var permission = "";

            dynamic requestData = new System.Dynamic.ExpandoObject();
            requestData.materialId = materialId;
            requestData.userId = userId;
            requestData.permission = permission;


            var result = _controller.SetPermission(requestData);


            var jsonResult = Assert.IsType<JsonResult>(result);
            var response = Newtonsoft.Json.Linq.JObject.FromObject(jsonResult.Value);

            Assert.NotNull(response);
            Console.WriteLine(response);
            Assert.False((bool)response.success);
            Assert.Equal("Permission cannot be empty.", (string)response.message);
        }

    }

}

