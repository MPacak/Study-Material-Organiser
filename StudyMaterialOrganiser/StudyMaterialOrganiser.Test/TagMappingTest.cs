using AutoMapper;
using BL.Models;
using StudyMaterialOrganiser.Mappers;
using StudyMaterialOrganiser.ViewModels;
using Xunit;

namespace StudyMaterialOrganiser.Test
{
    public class TagMappingTest
    {
        private readonly IMapper _mapper;

        public TagMappingTest()
        {
            // Configure AutoMapper with the MappingProfile
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public void CanMap_TagDto_To_TagVM()
        {
            // Arrange
            var tagDto = new TagDto
            {
                Id = 1,
                Name = "Test Tag",
                Groups = new List<string> { "Group1", "Group2" }
            };

            // Act
            var tagVM = _mapper.Map<TagVM>(tagDto);

            // Assert
            Assert.NotNull(tagVM);
            Assert.Equal(tagDto.Id, tagVM.Id);
            Assert.Equal(tagDto.Name, tagVM.Name);
            Assert.Equal(tagDto.Groups, tagVM.Groups);
        }

        [Fact]
        public void CanMap_TagVM_To_TagDto()
        {
            // Arrange
            var tagVM = new TagVM
            {
                Id = 1,
                Name = "Test Tag",
                Groups = new List<string> { "Group1", "Group2" }
            };

            // Act
            var tagDto = _mapper.Map<TagDto>(tagVM);

            // Assert
            Assert.NotNull(tagDto);
            Assert.Equal(tagVM.Id, tagDto.Id);
            Assert.Equal(tagVM.Name, tagDto.Name);
            Assert.Equal(tagVM.Groups, tagDto.Groups);
        }
    }
}
