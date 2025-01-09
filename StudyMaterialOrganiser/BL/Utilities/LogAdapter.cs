using BL.Models;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Utilities
{
	public static class LogAdapter
	{
		
		public static LogDto ToLogDto(Log log)
		{
			if (log == null) return null;

			return new LogDto
			{
				Id = log.Id,
				Level = log.Level,
				Message = log.Message,
				Timestamp = log.Timestamp
			};
		}

		public static IEnumerable<LogDto> ToLogDtoList(IEnumerable<Log> logs)
		{
			if (logs == null) return null;

			return logs.Select(ToLogDto).ToList();
		}

	
		public static Log ToLog(LogDto logDto)
		{
			if (logDto == null) return null;

			return new Log
			{
				Id = logDto.Id,
				Level = logDto.Level,
				Message = logDto.Message,
				Timestamp = logDto.Timestamp
			};
		}
	}

}
