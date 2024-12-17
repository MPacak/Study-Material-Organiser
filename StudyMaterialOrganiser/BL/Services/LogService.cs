using AutoMapper;
using BL.IServices;
using BL.Models;
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
        var logs =  _unitOfWork.Log.GetAll()
            .OrderByDescending(log => log.Timestamp)
            .Take(n)
            .Select(log => _logMapper.Map<LogDto>(log))
            .ToList();
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


}

