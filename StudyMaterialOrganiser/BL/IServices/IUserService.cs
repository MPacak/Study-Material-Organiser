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
        string GenerateToken(UserLoginDto request);

        void ChangePassword(UserPasswordChangeDto request);
        User Authenticate(string username, string password);
        UserDto GetByName(string name);
        UserDto GetByUserName(string username);
        UserDto GetByEmail(string email);


    }

