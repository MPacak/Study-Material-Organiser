using BL.IServices;
using BL.Models;
using BL.Security;
using DAL.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Repositories;

namespace BL.Services
{
	public class PasswordService : IPasswordService
	{


		
		public string GenerateSalt()
		{
			return PasswordHashProvider.GetSalt();
		}

		public string HashPassword(string password, string salt)
		{
			return PasswordHashProvider.GetHash(password, salt);
		}

		public bool VerifyPassword(string password, string hash, string salt)
		{
			var computedHash = PasswordHashProvider.GetHash(password, salt);
			return computedHash == hash;
		}

	
	}
}
