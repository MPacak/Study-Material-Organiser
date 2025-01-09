using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Utilities
{
    public class AdminRoleApprovalStrategy : IRoleApprovalStrategy
    {
        public bool CanApprove(int? currentRole) => currentRole == 0;

        public bool CanDisapprove(int? currentRole) => currentRole > 0 && currentRole < 2;
    }
}
