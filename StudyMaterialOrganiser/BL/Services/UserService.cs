using AutoMapper;
using BL.Builder;
using BL.IServices;
using BL.Models;
using BL.Security;
using DAL.IRepositories;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _userMapper;
    private readonly IAuthService _authService;

	public UserService(IUnitOfWork  unitOfWork,IMapper userMapper, IAuthService authService)
    {
	    
		_unitOfWork =  unitOfWork;
        _userMapper = userMapper;
        _authService = authService;
    }
    public IEnumerable<UserDto> SearchUsers(Func<UserSearchQueryBuilder, UserSearchQueryBuilder> buildQuery)
    {
        var builder = new UserSearchQueryBuilder(_unitOfWork, _userMapper);
        var query = buildQuery(builder);
        return query.BuildDto();
    }
    public ICollection<UserDto> GetAll()
    {
		var allUsers =  _unitOfWork.User.GetAll(u => u.IsDeleted == false);
        return _userMapper.Map<ICollection<UserDto>>(allUsers).ToList();


    }

    public UserDto GetById(int id)
    {
        var user = _unitOfWork.User.GetFirstOrDefault(u => u.Id == id && !u.IsDeleted);
        return user == null ? null : _userMapper.Map<UserDto>(user);
    }




    public UserDto Create(UserRegistrationDto user)
    {
        var existingUser =  _unitOfWork.User.GetFirstOrDefault(u => u.Username == user.UserName);
        if (existingUser != null)
            throw new InvalidOperationException("User already exists");
        existingUser =  _unitOfWork.User.GetFirstOrDefault(u => u.Email == user.Email);
        if (existingUser != null)
            throw new InvalidOperationException("Email already exists");

        bool isFirstUser =  _unitOfWork.User.GetFirstOrDefault(u => u.Role == 2) == null;
        var salt = PasswordHashProvider.GetSalt();
        var hash = PasswordHashProvider.GetHash(user.Password, salt);

        var securityKey = RandomNumberGenerator.GetBytes(256 / 8);
        var b64SecKey = Convert.ToBase64String(securityKey);

        var newUser = new User
        {
            Username = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PwdHash = hash,
            PwdSalt = salt,
            Phone = user.Phone,

            SecurityToken = b64SecKey,
            Role = isFirstUser ? 2 : 0
        };


         _unitOfWork.User.Add(newUser);
         _unitOfWork.Save();
        return _userMapper.Map<UserDto>(newUser);

    }

	public UserDto Update(int id, UserDto user)
	{
		var userToUpdate = _unitOfWork.User.GetFirstOrDefault(u => u.Id == id);
		if (userToUpdate == null)
			throw new InvalidOperationException("User does not exist");

		if (userToUpdate.IsDeleted)
			throw new InvalidOperationException("Cannot update deleted user");

		userToUpdate.FirstName = user.FirstName;
		userToUpdate.LastName = user.LastName;
		userToUpdate.Username = user.Username;
		userToUpdate.Email = user.Email;
		userToUpdate.Phone = user.Phone;
		if (user.Role.HasValue)
		{
			userToUpdate.Role = user.Role.Value;
		}

		_unitOfWork.Save();
		return _userMapper.Map<UserDto>(userToUpdate);
	}

	public UserDto Delete(int id)
    {
        var toDelete =  _unitOfWork.User.GetFirstOrDefault(u => u.Id == id && u.IsDeleted == false);
        if (toDelete == null) return null;


        toDelete.IsDeleted = true;
        toDelete.DeletedAt = DateTime.Now;
         _unitOfWork.Save();

        return _userMapper.Map<UserDto>(toDelete);
    }


    public UserDto GetByUserName(string username)
    {
        var user =  _unitOfWork.User.GetFirstOrDefault(u => u.Username == username);
        if (user == null) return null;
        return _userMapper.Map<UserDto>(user);
    }

    public UserDto GetByEmail(string email)
    {
        var user =  _unitOfWork.User.GetFirstOrDefault(u => u.Email == email);
        if (user == null) return null;
        return _userMapper.Map<UserDto>(user);
    }


    public UserDto GetByName(string name)
    {
        var user =  _unitOfWork.User.GetFirstOrDefault(u => u.Username == name);
        if (user == null) return null;
        return _userMapper.Map<UserDto>(user);


    }



    public void ChangePassword(UserPasswordChangeDto request)
    {
        var user = _authService.Authenticate(request.Username, request.OldPassword);
        if (user != null)
        {
            string b64Salt = PasswordHashProvider.GetSalt();
            string b64Hash = PasswordHashProvider.GetHash(request.NewPassword, b64Salt);
            user.PwdSalt = b64Salt;
            user.PwdHash = b64Hash;

             _unitOfWork.Save();
        }
        else
        {
            throw new InvalidOperationException("Invalid username or password");
        }
    }
}