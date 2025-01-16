using AutoMapper;
using BL.IServices;
using BL.Models;
using BL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StudyMaterialOrganiser.Controllers.UserModule
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
		public IActionResult List(int page = 1, int pageSize = 20)
		{
			try
			{
				var paginatedLogs = _logService.GetPaginatedLogs(page, pageSize);

				ViewBag.PageSize = paginatedLogs.PageSize;
				ViewBag.CurrentPage = paginatedLogs.CurrentPage;
				ViewBag.TotalPages = paginatedLogs.TotalPages;

				return View(paginatedLogs.Items);
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
