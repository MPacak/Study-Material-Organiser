using BL.Models;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IServices
{
	public interface IStudyGroupService
	{

		void Add(StudyGroupDto group);
		void Remove(StudyGroupDto group);
		IEnumerable<StudyGroupDto> GetAll();
        StudyGroup? GetGroupById(int Id);
		void Update(int id, StudyGroupDto data);
	}
}
