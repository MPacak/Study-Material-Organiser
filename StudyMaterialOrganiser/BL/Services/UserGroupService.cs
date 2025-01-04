using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BL.IServices;
using BL.Models;
using DAL.IRepositories;

namespace BL.Services
{
	internal class UserGroupService : IUserGroupService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _userGroupMapper;

		public UserGroupService(IUnitOfWork unitOfWork, IMapper UserGroupMapper)
		{
			_unitOfWork = unitOfWork;
			_userGroupMapper = UserGroupMapper;
		}

		public ICollection<UserGroupDto> GetAll()
		{

			var allUserGroups = _unitOfWork.UserGroup.GetAll(null, "Project,User");
			var UserGroupDtos = _userGroupMapper.Map<ICollection<UserGroupDto>>(allUserGroups);


			return UserGroupDtos.ToList();
		}



		public UserGroupDto? Create(UserGroupDto UserGroup)
		{
			try
			{
				var existingUserGroup = _unitOfWork.UserGroup.GetFirstOrDefault(a =>
					a.UserId == UserGroup.UserId && a.GroupId == UserGroup.GroupId);

				if (existingUserGroup != null)
				{
					throw new InvalidOperationException("UserGroup already exists");
				}

				var newUserGroup = _userGroupMapper.Map<DAL.Models.UserGroup>(UserGroup);

				_unitOfWork.UserGroup.Add(newUserGroup);
				_unitOfWork.Save();

				return _userGroupMapper.Map<UserGroupDto>(newUserGroup);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Exception: {ex.Message}");
				Console.WriteLine($"StackTrace: {ex.StackTrace}");

				throw; 
			}
		}

		public UserGroupDto? GetByUserAndGroup(UserGroupDto UserGroup)
		{
			var UserGroupExists = _unitOfWork.UserGroup.GetFirstOrDefault(a =>
				a.UserId == UserGroup.UserId && a.GroupId == UserGroup.GroupId);

			return UserGroupExists == null ? null : _userGroupMapper.Map<UserGroupDto>(UserGroupExists);
		}
		public UserGroupDto? Delete(int id)
		{
			var UserGroupToDelete = _unitOfWork.UserGroup.GetFirstOrDefault(a => a.Id == id);
			if (UserGroupToDelete == null) return null;

			_unitOfWork.UserGroup.Delete(UserGroupToDelete);
			_unitOfWork.Save();
			return _userGroupMapper.Map<UserGroupDto>(UserGroupToDelete);
		}

	}

}