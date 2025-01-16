using BL.Builder;
using BL.Models;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IServices;

    public interface IUserService
    {
        ICollection<UserDto> GetAll();
        UserDto GetById(int id);
        UserDto Create(UserRegistrationDto user);
        UserDto Update(int id, UserDto user);
        UserDto Delete(int id);
        void ChangePassword(UserPasswordChangeDto request);
        UserDto GetByName(string name);
        UserDto GetByUserName(string username);
        UserDto GetByEmail(string email);
    IEnumerable<UserDto> SearchUsers(Func<UserSearchQueryBuilder, UserSearchQueryBuilder> buildQuery);

}

