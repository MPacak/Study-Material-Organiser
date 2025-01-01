using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Utilities
{
        public interface IFileHandler
        {
           
            string SaveFile(IFormFile file, string uploadPath);
            void DeleteFile(string filePath);
        }

    
}
