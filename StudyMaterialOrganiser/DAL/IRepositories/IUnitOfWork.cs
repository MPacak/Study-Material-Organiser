using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IRepositories
{
    public interface IUnitOfWork
    {
		IUserGroupRepository UserGroup { get; }
		ILogRepository Log { get; }
        IUserRepository User { get; }
        ITagRepository Tag { get; }
        IMaterialRepository Material { get; }
        IMaterialTagRepository MaterialTag { get; }
        IStudyGroupRepository StudyGroup { get; }
        void Save();
    }
}
