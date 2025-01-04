using AutoMapper;
using BL.IServices;
using BL.Models;
using BL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyMaterialOrganiser.Utilities;
using StudyMaterialOrganiser.ViewModels;
using System.Collections.Generic;

namespace StudyMaterialOrganiser.Controllers.UserModule
{
    public class GroupController : Controller
	{
		private readonly ILogService _logService;
		private readonly IMapper _mapper;
		private readonly IGroupService _groupService;
		private readonly ILogger<UserController> _logger;
	





		public GroupController(IGroupService groupService, IMapper mapper, ILogger<UserController> logger, ILogService logService)
		{

			_groupService = groupService;
			_logService = logService;
			_mapper = mapper;
			_logger = logger;
		



		}
		// GET: GroupController
		public ActionResult Index()
        {
			var groups = _groupService.GetAll();
			var groupDtos = _mapper.Map<ICollection<GroupDto>>(groups);
			_logService.Log("Action", $"All users were fetched by {User.Identity.Name}");
			return View(groupDtos);
		}

        // GET: GroupController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

		// GET: GroupController/Create
		public ActionResult Create()
		{
			var viewModel = new GroupDto()
			{
				
			};
			return View(viewModel);
		}

		// POST: GroupController/Create
		[Authorize(Roles = "Admin")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(GroupDto model)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return View(model);
				}


				_groupService.Create(model);
				_logService.Log("Registration", $"Group: {model.Name} Created");


				TempData["ToastMessage"] = $"User: {model.Name} created successfully!";
				TempData["ToastType"] = "success";
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while creating a new group");
				TempData["ToastMessage"] = ex.Message;
				TempData["ToastType"] = "error";
				return View(model);
			}
		}

		// GET: GroupController/Edit/5
		public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: GroupController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: GroupController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: GroupController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
