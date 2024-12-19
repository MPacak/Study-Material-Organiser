using BL.Models;


namespace BL.IServices;

public interface IUserGroupService
{
    ICollection<UserGroupDto> GetAll();

    UserGroupDto? Create(UserGroupDto userGroup);
    UserGroupDto? GetByUserAndProject(UserGroupDto userGroup);
    UserGroupDto? UpdateStatus(int id, string status);
    UserGroupDto? Delete(int id);

}