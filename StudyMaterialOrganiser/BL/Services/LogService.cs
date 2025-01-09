using AutoMapper;
using BL.IServices;
using BL.Models;
using BL.Utilities;
using DAL.IRepositories;
using DAL.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services;

public class LogService : ILogService
{
    private readonly IUnitOfWork  _unitOfWork;
    private readonly IMapper _logMapper;
    private readonly IConfiguration _configuration;

    public LogService(IUnitOfWork  unitOfWork, IMapper logMapper, IConfiguration configuration)
    {
         _unitOfWork =  unitOfWork;
        _logMapper = logMapper;
        _configuration = configuration;
    }

	public ICollection<LogDto> GetLastN(int n)
	{
		var logs = _unitOfWork.Log.GetAll()
			.OrderByDescending(log => log.Timestamp)
			.Take(n)
			.ToList();

		return LogAdapter.ToLogDtoList(logs).ToList();
	}

	public IEnumerable<Log> GetAll()
    {
	    var logs =_unitOfWork.Log.GetAll();
        return logs;
    }


    public int GetCount() =>  _unitOfWork.Log.GetAll().Count();


    public void Log(string level, string message)
    {
        var log = new Log
        {
            Timestamp = DateTime.Now,
            Level = level,
            Message = message,

        };
         _unitOfWork.Log.Add(log);
         _unitOfWork.Save();

    }
	public PaginatedResultDto<LogDto> GetPaginatedLogs(int page, int pageSize)
	{
		var query = _unitOfWork.Log.GetAll().OrderBy(log => log.Timestamp);

		var totalLogs = query.Count();
		var logsToDisplay = pageSize == 0
			? query.ToList()
			: query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

		var logDtos = LogAdapter.ToLogDtoList(logsToDisplay).ToList();

		return new PaginatedResultDto<LogDto>
		{
			Items = logDtos,
			CurrentPage = page,
			PageSize = pageSize,
			TotalPages = pageSize == 0 ? 1 : (int)Math.Ceiling(totalLogs / (double)pageSize),
			TotalCount = totalLogs
		};
	}
}




