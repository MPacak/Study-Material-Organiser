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
            if (Enum.TryParse<FileType>(folderTypeName, out var folderType))
            {
                return (int)folderType;
            }
            throw new ArgumentException($"Invalid FolderTypeName: {folderTypeName}");
        }
    }
}
