using DAL.IRepositories;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace StudyMaterialOrganiser.Controllers
{
    public class StudyGroupController : Controller
    {

        private readonly IGroupRepository _studyGroupRepository;
        private readonly ITagRepository _tagRepository;

        public StudyGroupController(IGroupRepository studyGroupRepository, ITagRepository tagRepository)
        {
            _studyGroupRepository = studyGroupRepository;
            _tagRepository = tagRepository;
        }

        // GET: /StudyGroup/Index
        public IActionResult Index()
        {
            var studyGroups = _studyGroupRepository.GetAll();
            return View(studyGroups);
        }

        // GET: /StudyGroup/Create
        public IActionResult Create()
        {
            var tags = _tagRepository.GetAll()
                .Select(tag => new SelectListItem
                {
                    Value = tag.Idtag.ToString(),
                    Text = tag.TagName
                }).ToList();

            ViewData["Tags"] = tags;
            return View();
        }

        // POST: /StudyGroup/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Group group)
        {
            if (ModelState.IsValid)
            {
                group.Id = 0; 
                _studyGroupRepository.Add(group); 
                return RedirectToAction(nameof(Index));
            }

            var tags = _tagRepository.GetAll()
                        .Select(tag => new SelectListItem
                        {
                            Value = tag.Idtag.ToString(),
                            Text = tag.TagName
                        }).ToList();
            ViewData["Tags"] = tags;

            return View(group);
        }


    }
}
