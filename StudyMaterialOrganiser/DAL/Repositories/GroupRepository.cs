using DAL.Models;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IRepositories
{
    public class StudyGroupRepository : Repository<StudyGroup>, IStudyGroupRepository
    {
		public StudyGroupRepository(StudymaterialorganiserContext dbContext) : base(dbContext)
		{

		}
	}
}
