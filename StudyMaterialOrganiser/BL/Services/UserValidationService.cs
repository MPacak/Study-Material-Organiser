using BL.IServices;
using DAL.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
	public class UserValidationService : IUserValidationService
	{
		private readonly IUnitOfWork _unitOfWork;

		public UserValidationService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public bool IsUsernameAvailable(string username, int? userId = null)
		{
			var existingUser = _unitOfWork.User.GetFirstOrDefault(u => u.Username == username);
			return existingUser == null || existingUser.Id == userId;
		}

		public bool IsEmailAvailable(string email, int? userId = null)
		{
			var existingUser = _unitOfWork.User.GetFirstOrDefault(u => u.Email == email);
			return existingUser == null || existingUser.Id == userId;
		}
	}
}
