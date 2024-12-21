using DAL.IRepositories;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
	public class StudyGroupRepository : IRepository<Group>
    {
        private readonly StudymaterialorganiserContext _context;

        public StudyGroupRepository(StudymaterialorganiserContext context)
        {
            _context = context;

        }

        public void Add(Group group)
        {

            _context.Add(group);
            _context.SaveChanges();
        }

        public void Remove(Group group)
        {
            _context.Remove(group);
            _context.SaveChanges();
        }

        public IEnumerable<Group> GetAll()
        {
            return _context.Groups.ToList();
        }

        public Group? GetGroupById(int groupId)
        {
            return _context.Groups.FirstOrDefault(g => g.Id == groupId);
        }

        public void Update(int id, Group data)
        {
            var existingGroup = _context.Groups.FirstOrDefault(g => g.Id == id);
            if (existingGroup != null)
            {
                existingGroup.Name = data.Name;
                existingGroup.TagId = data.TagId;
                _context.Update(existingGroup);
                _context.SaveChanges();
            }
        }

		public IEnumerable<Group> GetAll(Expression<Func<Group, bool>>? filter = null, string? includeProperties = null)
		{
			throw new NotImplementedException();
		}

		public Group GetFirstOrDefault(Expression<Func<Group, bool>> filter, string? includeProperties = null)
		{
			throw new NotImplementedException();
		}

		public void Delete(Group entity)
		{
			throw new NotImplementedException();
		}

		public void DeleteRange(IEnumerable<Group> entities)
		{
			throw new NotImplementedException();
		}
	}
}
