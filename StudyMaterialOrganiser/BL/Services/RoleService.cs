using BL.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
	public class RoleService : IRoleService
	{
		public bool CanApproveRole(int? currentRole)
		{
			return currentRole < 1;
		}

		public bool CanDisapproveRole(int? currentRole)
		{
			return currentRole > 0;
		}

		public int GetApprovedRole(int? currentRole)
		{
			return 1;
		}

		public int GetDisapprovedRole(int? currentRole)
		{
			return 0;
		}
	}

}
