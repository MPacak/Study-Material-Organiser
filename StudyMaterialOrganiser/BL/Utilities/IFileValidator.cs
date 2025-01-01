using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Utilities
{
   public interface IFileValidator
    {
        bool IsValidFile(string fileName);
    }
}
