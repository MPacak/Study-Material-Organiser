using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IServices
{
	public interface IRoleService
	{
		bool CanApproveRole(int? currentRole);
		bool CanDisapproveRole(int? currentRole);
		int GetApprovedRole(int? currentRole);
		int GetDisapprovedRole(int? currentRole);
	}
}
