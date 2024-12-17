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

          
            User = new UserRepository(_context);
            Log = new LogRepository(_context);

        }   
        public IUserRepository User { get; private set; }
        public ILogRepository Log { get; private set; }


        public void Save()
        {
            _context.SaveChanges();
        }



    }
}
