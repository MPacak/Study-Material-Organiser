using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Utilities
{
    public class DefaultRoleApprovalStrategy : IRoleApprovalStrategy
    {
        public bool CanApprove(int? currentRole) => currentRole < 1;

        public bool CanDisapprove(int? currentRole) => currentRole > 0;
    }
}
