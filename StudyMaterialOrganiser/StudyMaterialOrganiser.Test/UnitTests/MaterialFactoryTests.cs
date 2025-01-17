using System;
using Xunit;
using StudyMaterialOrganiser.Utilities;
using DAL.Models;

namespace StudyMaterialOrganiser.Test.UnitTests
{

    public class MaterialFactoryTests
    {
        private readonly MaterialFactory _materialFactory;

        public MaterialFactoryTests()
        {
            _materialFactory = new MaterialFactory();
        }

        [Fact]
        public void GetFolderTypeName_ValidFolderTypeId_ReturnsCorrectName()
        {

            var folderTypeId = (int)FileType.PDF;

            var result = _materialFactory.GetFolderTypeName(folderTypeId);

            Assert.Equal("PDF", result);
        }

        [Fact]
        public void GetFolderTypeName_InvalidFolderTypeId_ReturnsUnknown()
        {
           
            var invalidFolderTypeId = -1;

           
            var result = _materialFactory.GetFolderTypeName(invalidFolderTypeId);

            
            Assert.Equal("Unknown", result);
        }

        [Fact]
        public void GetFolderTypeId_ValidFolderTypeName_ReturnsCorrectId()
        {
           
            var folderTypeName = ".pdf";

           
            var result = _materialFactory.GetFolderTypeId(folderTypeName);

            
            Assert.Equal((int)FileType.PDF, result);
        }

        [Fact]
        public void GetFolderTypeId_InvalidFolderTypeName_ThrowsArgumentException()
        {
           
            var invalidFolderTypeName = "invalidType";


            var exception = Assert.Throws<ArgumentException>(() => _materialFactory.GetFolderTypeId(invalidFolderTypeName));
            Assert.Contains($"Invalid FolderTypeName: {invalidFolderTypeName}", exception.Message);
        }
    }

}
