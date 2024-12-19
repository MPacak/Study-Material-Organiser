using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.IServices;
using BL.Models;

namespace BL.Services
{
	internal class GroupService : IGroupService
	{
		public ICollection<GroupDto> GetAll()
		{
			throw new NotImplementedException();
		}

		public GroupDto? GetById(int id)
		{
			throw new NotImplementedException();
		}

		public GroupDto? Create(GroupDto project)
		{
			throw new NotImplementedException();
		}

		public GroupDto? Update(int id, GroupDto project)
		{
			throw new NotImplementedException();
		}

		public GroupDto? Delete(int id)
		{
			throw new NotImplementedException();
		}

		public int GetCount()
		{
			throw new NotImplementedException();
		}

		public IEnumerable<GroupDto> GetFiltered(IEnumerable<GroupDto> projects, string? filterBy, string? filter)
		{
			throw new NotImplementedException();
		}
	}
}
