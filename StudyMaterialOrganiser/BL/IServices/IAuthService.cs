using BL.Models;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IServices
{
	public interface IAuthService
	{
		string GenerateToken(UserLoginDto request);
		User Authenticate(string username, string password);
		void SignIn(string username, int? role, int userId);
		void SignOut();
	}
}
