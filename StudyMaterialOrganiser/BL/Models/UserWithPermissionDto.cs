using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models
{
    public class UserWithPermissionDto : UserDto
    {
        public string Permission { get; set; } = "None";
    }

}
