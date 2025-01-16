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
           
            return true;
        }
        
        public List<UserDto> GetUsersWithAccess(int materialId)
        {
           
            return new List<UserDto>();
        }

        public string GetPermission(int materialId, int userId)
        {
            
            return "view";
        }

        
        public void AssignPermission(int materialId, int userId, string permissionType)
        {
            throw new NotImplementedException("Permissions are handled by the proxy");
        }
    }
}