using DAL.IRepositories;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {

        public TagRepository(StudymaterialorganiserContext context) : base(context)
        {

        }
    }

}
