using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Utilities
{

public class BinaryFileHandler : BaseFileHandler
    {
        public BinaryFileHandler(IConfiguration configuration) : base(configuration)
        {
        }

        public override bool IsValidFile(string fileName)
        {
            return FileTypeExtensions.GetFileTypeFromExtension(fileName) != null;
        }

        public override string SaveFile(IFormFile file, string uploadPath)
        {
            var fileName = Path.GetRandomFileName() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadPath, fileName);
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                byte[] fileBytes = memoryStream.ToArray();
                File.WriteAllBytes(filePath, fileBytes);
            }
            return fileName;
        }

        public override void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public override byte[] GetFile(string fileName)
        {
            string filePath = Path.Combine(_storageBasePath, fileName);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File {fileName} not found.");
            }

            return File.ReadAllBytes(filePath);
        }
    

        public override int GetFileTypeId(IFormFile file)
        {
            var fileType = FileTypeExtensions.GetFileTypeFromExtension(file.FileName);
            return (int)fileType.Value;
        }
    }
}
