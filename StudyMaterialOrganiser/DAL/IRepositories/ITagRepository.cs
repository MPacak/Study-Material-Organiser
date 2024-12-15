using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IRepositories
{
   
        public interface ITagRepository
        {
            Tag? GetTagById(int tagId);
            Tag? GetTagByName(string tagName);
            void Add(Tag tag);
            void Update(int id, Tag data);
            void Remove(Tag tag);
            IEnumerable<Tag> GetAll();
        }
    
}
