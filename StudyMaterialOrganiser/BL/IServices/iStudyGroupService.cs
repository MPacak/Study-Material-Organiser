using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IServices
{
	public interface iStudyGroupService
	{

		void Add(Group group);
		void Remove(Group group);
		IEnumerable<Group> GetAll();
		Group? GetGroupById(int Id);
		void Update(int id, Group data);
	}
}
