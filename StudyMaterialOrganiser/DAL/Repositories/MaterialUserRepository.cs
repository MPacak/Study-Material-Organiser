using DAL.IRepositories;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
   public class MaterialUserRepository : Repository<MaterialUser>, IMaterialUserRepository
    {
        public MaterialUserRepository(StudymaterialorganiserContext context) : base(context)
        {

        }
    }
}
