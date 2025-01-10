using BL.IServices;
using BL.Models;
using DAL.IRepositories;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class MaterialAccessService : IMaterialAccessService
    {
        public bool CanAccessMaterial(int materialId, int userId, string permissionType)
        {
            // For demonstration, always return true
            return true;
        }

        public List<UserDto> GetUsersWithAccess(int materialId)
        {
            // Return empty list for demonstration
            return new List<UserDto>();
        }

        public string GetPermission(int materialId, int userId)
        {
            // For demonstration, return a default permission
            return "view";
        }

        // This method isn't actually implemented in the real service
        // as it's handled by the proxy
        public void AssignPermission(int materialId, int userId, string permissionType)
        {
            throw new NotImplementedException("Permissions are handled by the proxy");
        }
    }
}