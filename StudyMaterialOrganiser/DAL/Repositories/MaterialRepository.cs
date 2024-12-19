using DAL.IRepositories;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class MaterialRepository : Repository<Material>, IMaterialRepository
    {
      
        public MaterialRepository(StudymaterialorganiserContext context) : base(context)
        {
           
        }

       
    }
}
