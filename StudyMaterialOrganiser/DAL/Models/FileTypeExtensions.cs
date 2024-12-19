using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public static class FileTypeExtensions
    {
        private static readonly Dictionary<FileType, string[]> FileExtensions = new()
    {
        { FileType.PDF, new[] { ".pdf" } },
        { FileType.Word, new[] { ".doc" } },
        { FileType.Excel, new[] { ".xls" } },
        { FileType.PowerPoint, new[] { ".ppt" } },
        { FileType.JPEG, new[] { ".jpg", ".jpeg" } },
        { FileType.PNG, new[] { ".png" } },
        { FileType.WordX, new[] { ".docx" } },
        { FileType.ExcelX, new[] { ".xlsx" } },
        { FileType.PowerPointX, new[] { ".pptx" } }
    };

        public static bool IsValidExtension(this FileType fileType, string fileName)
        {
            var extension = Path.GetExtension(fileName)?.ToLowerInvariant();
            return FileExtensions[fileType].Contains(extension);
        }

        public static FileType? GetFileTypeFromExtension(string fileName)
        {
            var extension = Path.GetExtension(fileName)?.ToLowerInvariant();
            foreach (var fileType in FileExtensions)
            {
                if (fileType.Value.Contains(extension))
                {
                    return fileType.Key;
                }
            }
            return null;
        }
        public static int GetFileTypeId(string fileName)
        {
            var fileType = GetFileTypeFromExtension(fileName);
            return fileType.HasValue ? (int)fileType.Value : 0;
        }
    }
    }
