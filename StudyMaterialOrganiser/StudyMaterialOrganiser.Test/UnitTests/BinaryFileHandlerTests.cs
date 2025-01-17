using BL.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.IO;
using System.Text;
using Xunit;

namespace StudyMaterialOrganiser.Test.UnitTests
{
    

    public class BinaryFileHandlerTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly BinaryFileHandler _fileHandler;
        private readonly string _testStoragePath;

        public BinaryFileHandlerTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _testStoragePath = Path.Combine(Path.GetTempPath(), "BinaryFileHandlerTests");

            _mockConfiguration.Setup(c => c["FileStorage:BasePath"]).Returns(_testStoragePath);

            _fileHandler = new BinaryFileHandler(_mockConfiguration.Object);

           
            if (Directory.Exists(_testStoragePath))
            {
                Directory.Delete(_testStoragePath, true);
            }
            Directory.CreateDirectory(_testStoragePath);
        }

        [Fact]
        public void IsValidFile_ValidExtension_ReturnsTrue()
        {
            // Arrange
            var validFileName = "test.pdf";

            // Act
            var result = _fileHandler.IsValidFile(validFileName);

            // Assert
            Assert.True(result, "Expected IsValidFile to return true for a valid file extension.");
        }

        [Fact]
        public void IsValidFile_InvalidExtension_ReturnsFalse()
        {
            
            var invalidFileName = "test.invalid";

     
            var result = _fileHandler.IsValidFile(invalidFileName);

    
            Assert.False(result, "Expected IsValidFile to return false for an invalid file extension.");
        }

        [Fact]
        public void SaveFile_ValidFile_SavesFileAndReturnsFileName()
        {
           
            var fileName = "testfile.pdf";
            var fileContent = Encoding.UTF8.GetBytes("Test file content");
            var formFile = new FormFile(new MemoryStream(fileContent), 0, fileContent.Length, "Data", fileName);

      
            var result = _fileHandler.SaveFile(formFile, _testStoragePath);

          
            var savedFilePath = Path.Combine(_testStoragePath, result);
            Assert.True(File.Exists(savedFilePath), "Expected the file to be saved at the specified location.");
            var savedContent = File.ReadAllBytes(savedFilePath);
            Assert.Equal(fileContent, savedContent);
        }

        [Fact]
        public void DeleteFile_FileExists_DeletesFile()
        {
       
            var fileName = "testfile.pdf";
            var filePath = Path.Combine(_testStoragePath, fileName);
            File.WriteAllText(filePath, "Dummy content");


            _fileHandler.DeleteFile(filePath);

   
            Assert.False(File.Exists(filePath), "Expected the file to be deleted.");
        }

        [Fact]
        public void DeleteFile_FileDoesNotExist_NoExceptionThrown()
        {
        
            var nonExistentFilePath = Path.Combine(_testStoragePath, "nonexistentfile.pdf");

         
            var exception = Record.Exception(() => _fileHandler.DeleteFile(nonExistentFilePath));
            Assert.Null(exception); 
        }

        [Fact]
        public void GetFile_FileExists_ReturnsFileContent()
        {
         
            var fileName = "testfile.pdf";
            var fileContent = Encoding.UTF8.GetBytes("Test file content");
            var filePath = Path.Combine(_testStoragePath, fileName);
            File.WriteAllBytes(filePath, fileContent);

     
            var result = _fileHandler.GetFile(fileName);

            Assert.Equal(fileContent, result);
        }

        [Fact]
        public void GetFile_FileDoesNotExist_ThrowsFileNotFoundException()
        {

            var nonExistentFileName = "nonexistentfile.pdf";

            var exception = Assert.Throws<FileNotFoundException>(() => _fileHandler.GetFile(nonExistentFileName));
            Assert.Contains(nonExistentFileName, exception.Message);
        }

        [Fact]
        public void GetFileTypeId_ValidFile_ReturnsCorrectId()
        {
  
            var validFileName = "test.pdf";
            var fileContent = Encoding.UTF8.GetBytes("Test file content");
            var formFile = new FormFile(new MemoryStream(fileContent), 0, fileContent.Length, "Data", validFileName);

            var result = _fileHandler.GetFileTypeId(formFile);

            Assert.True(result > 0, "Expected a valid file type ID.");
        }

        [Fact]
        public void GetFileTypeId_InvalidFile_ThrowsException()
        {

            var invalidFileName = "test.invalid";
            var fileContent = Encoding.UTF8.GetBytes("Test file content");
            var formFile = new FormFile(new MemoryStream(fileContent), 0, fileContent.Length, "Data", invalidFileName);

            Assert.Throws<InvalidOperationException>(() => _fileHandler.GetFileTypeId(formFile));
        }
    }

}
