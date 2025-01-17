using AutoMapper;
using BL.IServices;
using BL.Models;
using BL.Services;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StudyMaterialOrganiser.Controllers;
using StudyMaterialOrganiser.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace StudyMaterialOrganiser.Test.TagTests
{
    public class TagControllerIntegrationTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;
        private readonly TagController _controller;

        public TagControllerIntegrationTests(TestFixture fixture)
        {
 
            var tagService = fixture.ServiceProvider.GetRequiredService<ITagService>();
            var mapper = fixture.ServiceProvider.GetRequiredService<IMapper>();
            _fixture = fixture;
            _controller = new TagController(tagService, mapper);
        }

        [Fact]
        public async Task Index_ShouldReturnViewWithTags()
        {
     
            var dbContext = _fixture.DbContext;
            dbContext.Tags.AddRange(new Tag { TagName = "Tag1" }, new Tag { TagName = "Tag2" });
            await dbContext.SaveChangesAsync();

            var result = _controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<TagVM>>(viewResult.Model);
            Assert.Equal(2, model.Count());
            Assert.Contains(model, tag => tag.Name == "Tag1");
            Assert.Contains(model, tag => tag.Name == "Tag2");
        }

        [Fact]
        public void Create_Get_ShouldReturnEmptyTagVM()
        {

            var result = _controller.Create();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<TagVM>(viewResult.Model);
            Assert.NotNull(model);
            Assert.Null(model.Name); // Name should be empty for a new tag
        }

        [Fact]
        public async Task Create_Post_ShouldAddTagAndRedirect()
        {

            var dbContext = _fixture.DbContext;
            var tagVm = new TagVM { Name = "NewTag" };

            var result = _controller.Create(tagVm);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(_controller.Index), redirectResult.ActionName);

            var createdTag = await dbContext.Tags.FirstOrDefaultAsync(t => t.TagName == "NewTag");
            Assert.NotNull(createdTag);
        }

        [Fact]
        public async Task Edit_Get_ShouldReturnViewWithTagVM()
        {

            var dbContext = _fixture.DbContext;
            var tag = new Tag { TagName = "TagToEdit" };
            dbContext.Tags.Add(tag);
            await dbContext.SaveChangesAsync();


            var result = _controller.Edit(tag.Idtag);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<TagVM>(viewResult.Model);
            Assert.Equal(tag.TagName, model.Name);
        }

        [Fact]
        public async Task Edit_Post_ShouldUpdateTagAndRedirect()
        {

            var dbContext = _fixture.DbContext;
            var tag = new Tag { TagName = "OldName" };
            dbContext.Tags.Add(tag);
            await dbContext.SaveChangesAsync();

            var updatedTagVm = new TagVM { Id = tag.Idtag, Name = "UpdatedName" };

            var result = _controller.Edit(updatedTagVm);


            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(_controller.Index), redirectResult.ActionName);

            var updatedTag = await dbContext.Tags.FirstOrDefaultAsync(t => t.Idtag == tag.Idtag);
            Assert.NotNull(updatedTag);
            Assert.Equal("UpdatedName", updatedTag.TagName);
        }

        [Fact]
        public async Task Delete_Post_ShouldRemoveTagAndRedirect()
        {

            var dbContext = _fixture.DbContext;
            var tag = new Tag { TagName = "TagToDelete" };
            dbContext.Tags.Add(tag);
            await dbContext.SaveChangesAsync();


            var result = _controller.Delete(tag.Idtag);


            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(_controller.Index), redirectResult.ActionName);

            var deletedTag = await dbContext.Tags.FirstOrDefaultAsync(t => t.Idtag == tag.Idtag);
            Assert.Null(deletedTag);
        }
    }
}
