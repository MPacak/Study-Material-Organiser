using DAL.IRepositories;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class TagRepository :ITagRepository
    {
        private readonly StudymaterialorganiserContext _context;

        public TagRepository(StudymaterialorganiserContext context)
        {
            _context = context;
        }

        public void Add(Tag tag)
        {
            _context.Add(tag);
        }

        public void Remove(Tag tag)
        {
            _context.Remove(tag);
        }

        public IEnumerable<Tag> GetAll()
        {
            return _context.Tags;
        }

        public Tag? GetTagById(int tagId)
        {
            return _context.Tags.FirstOrDefault(tag => tag.Idtag == tagId);
        }

        public Tag? GetTagByName(string tagName)
        {
            return _context.Tags.FirstOrDefault(tag => tag.TagName == tagName);
        }

        public void Update(int id, Tag data)
        {
            var existingTag = _context.Tags.FirstOrDefault(tag => tag.Idtag == id);
            if (existingTag != null)
            {
                existingTag.TagName = data.TagName;
                _context.Update(existingTag);
            }
        }
    }

}
