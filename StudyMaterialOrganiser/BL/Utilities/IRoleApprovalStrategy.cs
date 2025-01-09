using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Utilities
{
    public interface IRoleApprovalStrategy
    {
        bool CanApprove(int? currentRole);
        bool CanDisapprove(int? currentRole);
    }
}
