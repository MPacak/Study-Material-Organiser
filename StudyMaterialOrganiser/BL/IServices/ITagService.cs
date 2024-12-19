using BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IServices
{
    public interface ITagService
    {
        IEnumerable<TagDto> GetAll();
        TagDto GetById(int id);
        TagDto GetByName(string name);
        void Create(TagDto tagDto);
        void Update(TagDto tagDto);
        void Delete(int id);
        bool Exists(string name);
    }
}
