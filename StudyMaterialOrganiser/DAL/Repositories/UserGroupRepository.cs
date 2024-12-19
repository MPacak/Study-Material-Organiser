using DAL.Models;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.IRepositories;

namespace DAL.Repositories
{
	public class UserGroupRepository : Repository<UserGroup>, IUserGroupRepository
	{
		public UserGroupRepository(StudymaterialorganiserContext dbContext) : base(dbContext)
		{

		}
	
		}
}
