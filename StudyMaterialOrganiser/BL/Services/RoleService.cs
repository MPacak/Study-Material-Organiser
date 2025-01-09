using BL.IServices;
using BL.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class RoleService : IRoleService
	{
		private readonly IRoleApprovalStrategy _roleApprovalStrategy;

		public RoleService(IRoleApprovalStrategy roleApprovalStrategy)
		{
			_roleApprovalStrategy = roleApprovalStrategy;
		}

		public bool CanApproveRole(int? currentRole)
		{
			return _roleApprovalStrategy.CanApprove(currentRole);
		}

		public bool CanDisapproveRole(int? currentRole)
		{
			return _roleApprovalStrategy.CanDisapprove(currentRole);
		}

		public int GetApprovedRole(int? currentRole) => 1;

		public int GetDisapprovedRole(int? currentRole) => 0;
	}

}
