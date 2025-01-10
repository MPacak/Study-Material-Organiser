using DAL.Models;

namespace StudyMaterialOrganiser.Utilities
{
    public class MaterialFactory : IMaterialFactory
    {
        public string GetFolderTypeName(int folderTypeId)
        {
            return Enum.GetName(typeof(FileType), folderTypeId) ?? "Unknown";
        }

        public int GetFolderTypeId(string folderTypeName)
        {

            var fileType = FileTypeExtensions.GetFileTypeFromExtension(folderTypeName);

            if (fileType.HasValue)
            {
                return (int)fileType.Value; 
            }

            throw new ArgumentException($"Invalid FolderTypeName: {folderTypeName}");
        }
    }
}
