using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.IServices;
using BL.Models;

namespace BL.Services
{
	internal class UserGroupService : IUserGroupService
	{
		public ICollection<UserGroupDto> GetAll()
		{
			throw new NotImplementedException();
		}

		public UserGroupDto? Create(UserGroupDto userGroup)
		{
			throw new NotImplementedException();
		}

		public UserGroupDto? GetByUserAndProject(UserGroupDto userGroup)
		{
			throw new NotImplementedException();
		}

		public UserGroupDto? UpdateStatus(int id, string status)
		{
			throw new NotImplementedException();
		}

		public UserGroupDto? Delete(int id)
		{
			throw new NotImplementedException();
		}
	}
}
