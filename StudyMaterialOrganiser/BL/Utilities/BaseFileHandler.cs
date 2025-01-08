using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Utilities
{
    public abstract class BaseFileHandler
    {
        protected readonly string _storageBasePath;

        protected BaseFileHandler(IConfiguration configuration)
        {
            _storageBasePath = configuration["FileStorage:BasePath"]
                ?? Path.Combine(Directory.GetCurrentDirectory(), "BinaryStorage");
            if (!Directory.Exists(_storageBasePath))
            {
                Directory.CreateDirectory(_storageBasePath);
            }
        }

        public abstract string SaveFile(IFormFile file, string uploadPath);
        public abstract void DeleteFile(string filePath);
        public abstract byte[] GetFile(string fileName);
        public abstract bool IsValidFile(string fileName);
        public abstract int GetFileTypeId(IFormFile file);
    }
}
