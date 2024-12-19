using DAL.IRepositories;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StudymaterialorganiserContext _context;

        public UnitOfWork(StudymaterialorganiserContext context)
        {
            _context = context;

            UserGroup = new UserGroupRepository(_context);
            Group = new GroupRepository(_context);
            User = new UserRepository(_context);
            Log = new LogRepository(_context);
            Material = new MaterialRepository(_context);
            Tag = new TagRepository(_context);
            MaterialTag = new MaterialTagRepository(_context);

        }


        public IUserGroupRepository UserGroup { get; private set; }
		public IGroupRepository Group { get; private set; }
        public IUserRepository User { get; private set; }
        public ILogRepository Log { get; private set; }

        public IMaterialRepository Material { get; private set; }
        public ITagRepository Tag { get; private set; }
        public IMaterialTagRepository MaterialTag { get; private set; }

       

        public void Save()
        {
            _context.SaveChanges();
        }



    }
}
