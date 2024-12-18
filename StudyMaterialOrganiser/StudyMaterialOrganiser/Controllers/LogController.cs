using AutoMapper;
using BL.IServices;
using BL.Models;
using BL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StudyMaterialOrganiser.Controllers
{
	public class LogController : Controller
	{
        private readonly ILogService _logService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public LogController(IMapper mapper, ILogger<UserController> logger, ILogService logService)
        {

            
            _logService = logService;
            _mapper = mapper;
            _logger = logger;



        }
        public ActionResult List(int page = 1, int pageSize = 20)
        {
	        try
	        {
		        
		        var allLogs = _logService.GetAll();

		       
		        var totalLogs = allLogs.Count();
		        var logsToDisplay = (pageSize == 0)
			        ? allLogs.OrderBy(log => log.Timestamp).ToList() 
			        : allLogs.OrderBy(log => log.Timestamp)
				        .Skip((page - 1) * pageSize)
				        .Take(pageSize)
				        .ToList();

		       
		        var logDtos = _mapper.Map<List<LogDto>>(logsToDisplay);

		     
		        ViewBag.PageSize = pageSize;
		        ViewBag.CurrentPage = page;
		        ViewBag.TotalPages = (pageSize == 0) ? 1 : (int)Math.Ceiling(totalLogs / (double)pageSize);

		        return View(logDtos);
	        }
	        catch (Exception ex)
	        {
		        _logger.LogError(ex, "An error occurred while fetching the list of logs.");
		        TempData["ToastMessage"] = "An error occurred while processing your request.";
		        TempData["ToastType"] = "error";
		        return RedirectToAction("Error", "Home");
	        }
        }
	}
}
