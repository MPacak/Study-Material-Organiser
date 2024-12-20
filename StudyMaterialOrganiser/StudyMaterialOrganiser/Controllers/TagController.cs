using AutoMapper;
using BL.IServices;
using BL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyMaterialOrganiser.ViewModels;

namespace StudyMaterialOrganiser.Controllers
{
    public class TagController : Controller
    {
        private readonly ITagService _tagService;
        private readonly IMapper _mapper;

        public TagController(ITagService tagService, IMapper mapper)
        {
            _tagService = tagService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var tags = _tagService.GetAll();
            var tagVm = tags.Select(x => _mapper.Map<TagVM>(x));
            return View(tagVm);
        }

        public IActionResult Create()
        {
            return View(new TagVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TagVM tagVm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var tagDto = _mapper.Map<TagDto>(tagVm);
                    _tagService.Create(tagDto);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("Name", ex.Message);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error creating tag: {ex.Message}");
            }

            return View(tagVm);
        }

        public IActionResult Edit(int id)
        {
            try
            {
                var tag = _tagService.GetById(id);
                var tagVm = _mapper.Map<TagVM>(tag);
                return View(tagVm);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TagVM tagVm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var tagDto = _mapper.Map<TagDto>(tagVm);
                    _tagService.Update(tagDto);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("Name", ex.Message);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error updating tag: {ex.Message}");
            }

            return View(tagVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            try
            {
                _tagService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
