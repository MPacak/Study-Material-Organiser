using BL.Utilities;
using DAL.Models;
using Microsoft.AspNetCore.Http;

namespace StudyMaterialOrganiser.Utilities
{
    public class FileHandler :IFileHandler, IFileValidator
    {
        public bool IsValidFile(string fileName)
        {
            return FileTypeExtensions.GetFileTypeFromExtension(fileName) != null;
        }

        public string SaveFile(IFormFile file, string uploadPath)
        {
            var fileName = Path.GetRandomFileName() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadPath, fileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(stream);
            return fileName;
        }

        public void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
