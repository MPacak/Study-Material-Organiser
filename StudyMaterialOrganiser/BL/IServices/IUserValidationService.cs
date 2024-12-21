using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IServices
{
	public interface IUserValidationService
	{
		bool IsUsernameAvailable(string username, int? userId = null);
		bool IsEmailAvailable(string email, int? userId = null);
	}
}
