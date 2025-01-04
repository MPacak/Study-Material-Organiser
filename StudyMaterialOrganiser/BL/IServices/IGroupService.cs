using BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace BL.IServices;

public interface IGroupService
{
	ICollection<GroupDto> GetAll();
	GroupDto? GetById(int id);

	GroupDto? Create(GroupDto project);
	GroupDto? Update(int id, GroupDto project);
	GroupDto? Delete(int id);
	int GetCount();

	


}