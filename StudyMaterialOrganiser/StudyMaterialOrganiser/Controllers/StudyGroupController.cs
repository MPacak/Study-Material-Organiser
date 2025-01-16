using BL.IServices;
using BL.Models;
using DAL.IRepositories;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace StudyMaterialOrganiser.Controllers
{
    public class StudyGroupController : Controller
    {

        private readonly IStudyGroupService _studyGroupService;
        private readonly ITagService _tagService;

        public StudyGroupController(IStudyGroupService studyGroupService, ITagService tagService)
        {
            _studyGroupService = studyGroupService;
            _tagService = tagService;
        }

        // GET: /StudyGroup/Index
        public IActionResult Index()
        {
            var studyGroupDtos = _studyGroupService.GetAll();
            var studyGroups = studyGroupDtos.Select(dto => new StudyGroup
            {
                Id = dto.Id,
                Name = dto.Name,
                TagId = dto.TagId,
                Tag = new Tag { TagName = dto.TagName }
            }).ToList();

            return View(studyGroups);
        }

        // GET: /StudyGroup/Create
        public IActionResult Create()
        {
            var tags = _tagService.GetAll()
                .Select(tag => new SelectListItem
                {
                    Value = tag.Id.ToString(),
                    Text = tag.Name
                }).ToList();

            ViewData["Tags"] = tags;
            return View();
        }

        // POST: /StudyGroup/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(StudyGroupDto group)
        {
            if (ModelState.IsValid)
            {
                group.Id = 0; 
                _studyGroupService.Add(group); 
                return RedirectToAction(nameof(Index));
            }

            var tags = _tagService.GetAll()
                        .Select(tag => new SelectListItem
                        {
                            Value = tag.Id.ToString(),
                            Text = tag.Name
                        }).ToList();
            ViewData["Tags"] = tags;

            return View(group);
        }


    }
}
