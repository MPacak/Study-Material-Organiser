using Microsoft.AspNetCore.Mvc;
using StudyMaterialOrganiser.ViewModels;
using System.Diagnostics;

namespace StudyMaterialOrganiser.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			if (User.IsInRole("Admin")) return RedirectToAction("List", "User");
			if (User.IsInRole("User")) return View("Home", "Home");
			if (User.IsInRole("NonUser")) return View("Home", "Home");

			return View();



		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
