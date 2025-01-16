using BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace BL.IServices;

public interface ILogService
{
	PaginatedResultDto<LogDto> GetPaginatedLogs(int page, int pageSize);

	ICollection<LogDto> GetLastN(int n);
    IEnumerable<Log> GetAll();
    int GetCount();
    void Log(string level, string message);


}

