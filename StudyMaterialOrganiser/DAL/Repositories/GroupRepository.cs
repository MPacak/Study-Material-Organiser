using DAL.Models;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IRepositories
{
    public class GroupRepository : Repository<Group>, IGroupRepository
    {
		public GroupRepository(StudymaterialorganiserContext dbContext) : base(dbContext)
		{

		}
	}
}
