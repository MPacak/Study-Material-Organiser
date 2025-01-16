using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Utilities
{
    public interface IBinaryFileHandler
    {
        bool IsValidFile(string fileName);
        string SaveFile(IFormFile file, string uploadPath);
        void DeleteFile(string filePath);
        byte[] GetFile(string fileName);
         int GetFileTypeId(IFormFile file);
    }
}
