using Xunit;
using StudyMaterialOrganiser.Utilities;
using StudyMaterialOrganiser.ViewModels;
using BL.Services;
using BL.IServices;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using DAL.Models;
using System.Collections.Generic;
using System.Linq;
using DAL.IRepositories;
using Microsoft.Extensions.Configuration;

namespace StudyMaterialOrganiser.Test
{
    public class AssignTagsTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;


        public AssignTagsTests(TestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void AssignTag_ReturnsMappedTagVMs()
        {
            
            var dbContext = _fixture.DbContext;
            var mapper = _fixture.ServiceProvider.GetRequiredService<IMapper>();
            var unitOfWork = _fixture.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var configuration = _fixture.ServiceProvider.GetRequiredService<IConfiguration>();
            var tagService = new TagService(unitOfWork, configuration,mapper);

          
            dbContext.Tags.AddRange(new List<Tag>
            {
                new Tag { TagName = "Tag1" },
                new Tag { TagName = "Tag2" },
                new Tag { TagName = "Tag3" }
            });
            dbContext.SaveChanges();

            
            var assignTags = new AssignTags(tagService, mapper);

            
            var result = assignTags.AssignTag();

      
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);

            var tagNames = result.Select(t => t.Name).ToList();
            Assert.Contains("Tag1", tagNames);
            Assert.Contains("Tag2", tagNames);
            Assert.Contains("Tag3", tagNames);
        }
    }
}
