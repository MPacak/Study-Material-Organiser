using BL.Models;


namespace BL.IServices;

public interface IUserGroupService
{
    ICollection<UserGroupDto> GetAll();

    UserGroupDto? Create(UserGroupDto userGroup);
    UserGroupDto? GetByUserAndGroup(UserGroupDto userGroup);
    UserGroupDto? Delete(int id);

}