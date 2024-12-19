using DAL.IRepositories;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class MaterialTagRepository : Repository<MaterialTag>, IMaterialTagRepository
    {
        public MaterialTagRepository(StudymaterialorganiserContext context) : base(context)
        {

        }
    }
}
