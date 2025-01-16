namespace StudyMaterialOrganiser.Utilities
{
    public interface IMaterialFactory
    {
        string GetFolderTypeName(int folderTypeId);
        int GetFolderTypeId(string folderTypeName);
    }
}
