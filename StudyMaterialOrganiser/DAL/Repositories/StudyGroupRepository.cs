using DAL.IRepositories;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class StudyGroupRepository : Repository<StudyGroup>, IStudyGroupRepository
    {
        public StudyGroupRepository(StudymaterialorganiserContext context) : base(context)
        {
        }
    }
}
