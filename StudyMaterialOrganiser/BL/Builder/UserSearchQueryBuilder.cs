using AutoMapper;
using BL.Models;
using DAL.IRepositories;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Builder
{
    public class UserSearchQueryBuilder
    {
        private IQueryable<User> _query;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper; 

        public UserSearchQueryBuilder(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _query = _unitOfWork.User.GetAll().AsQueryable();
            _mapper = mapper;
        }

        public UserSearchQueryBuilder FilterByName(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                _query = _query.Where(u => u.FirstName.ToLower().Contains(name, StringComparison.OrdinalIgnoreCase) ||
                u.LastName.ToLower().Contains(name, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(u => u.FirstName).ThenBy(u => u.LastName);
               
            }
         
            return this;
        }

        public UserSearchQueryBuilder FilterByEmail(string email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                _query = _query.Where(u => u.Email.Contains(email, StringComparison.OrdinalIgnoreCase));
            }
            return this;
        }

        public IEnumerable<UserDto> BuildDto()
        {
            var users = _query.ToList();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public IEnumerable<User> Build()
        {
            return _query.ToList();
        }
    }
}
