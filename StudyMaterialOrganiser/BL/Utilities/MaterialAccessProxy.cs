using BL.IServices;
using BL.Models;
using BL.Services;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Utilities
{
    public class MaterialAccessProxy : IMaterialAccessService
    {
        private readonly MaterialAccessService _realService;
        private readonly Dictionary<int, Dictionary<int, string>> _permissions;

        public MaterialAccessProxy(MaterialAccessService realService)
        {
            _realService = realService;
            _permissions = new Dictionary<int, Dictionary<int, string>>();
        }

        public bool CanAccessMaterial(int materialId, int userId, string permissionType)
        {
            // Check in-memory permissions first
            if (_permissions.TryGetValue(materialId, out var userPermissions) &&
                userPermissions.TryGetValue(userId, out var permission))
            {
                // If user has edit permission, they can also view
                if (permission == "edit" && permissionType == "view")
                    return true;

                return permission == permissionType;
            }

            // If no in-memory permission found, return false
            return false;
        }

        public List<UserDto> GetUsersWithAccess(int materialId)
        {
            var baseUsers = _realService.GetUsersWithAccess(materialId);
            var usersWithPermissions = new List<UserDto>();

            foreach (var baseUser in baseUsers)
            {
                var userWithPermission = new UserWithPermissionDto
                {
                    Id = baseUser.Id,
                    FirstName = baseUser.FirstName,
                    LastName = baseUser.LastName,
                    Email = baseUser.Email
                };

                if (_permissions.TryGetValue(materialId, out var materialPermissions) &&
                    materialPermissions.TryGetValue(baseUser.Id, out var permission))
                {
                    userWithPermission.Permission = permission;
                }

                usersWithPermissions.Add(userWithPermission);
            }

            return usersWithPermissions;
        }

        public void AssignPermission(int materialId, int userId, string permissionType)
        {
            // Validate permission type
            if (permissionType != "view" && permissionType != "edit")
            {
                throw new ArgumentException("Invalid permission type. Must be 'view' or 'edit'.");
            }

            // Ensure the material dictionary exists
            if (!_permissions.ContainsKey(materialId))
            {
                _permissions[materialId] = new Dictionary<int, string>();
            }

            // Set or update the permission
            _permissions[materialId][userId] = permissionType;
        }

        public string GetPermission(int materialId, int userId)
        {
            if (_permissions.TryGetValue(materialId, out var userPermissions) &&
                userPermissions.TryGetValue(userId, out var permission))
            {
                return permission;
            }

            return "None";
        }
    }
}

