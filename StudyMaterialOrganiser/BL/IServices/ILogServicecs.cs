using BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IServices;

public interface ILogService
{
    ICollection<LogDto> GetLastN(int n);
    int GetCount();
    void Log(string level, string message);


}