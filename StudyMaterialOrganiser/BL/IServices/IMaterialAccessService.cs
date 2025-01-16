using BL.Models;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IServices
{
    public interface IMaterialAccessService
    {
        bool CanAccessMaterial(int materialId, int userId, string permissionType);
        List<UserDto> GetUsersWithAccess(int materialId);
        void AssignPermission(int materialId, int userId, string permissionType);
        string GetPermission(int materialId, int userId);
    }
}
