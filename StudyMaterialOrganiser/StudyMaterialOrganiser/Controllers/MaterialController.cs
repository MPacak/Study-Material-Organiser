using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DAL.IRepositories;
using AutoMapper;
using StudyMaterialOrganiser.ViewModels;
using DAL.Models;

namespace StudyMaterialOrganiser.Controllers
{
    public class MaterialController : Controller
    {
 
        private readonly IMaterialRepository _materialRepository;
        private readonly IMapper _mapper;

        public MaterialController(IMaterialRepository materialRepository, IMapper mapper)
        {
            _materialRepository = materialRepository;
            _mapper = mapper;
        }


        // GET: MaterialController
        public ActionResult Index()
        {
            return View();
        }

        // GET: MaterialController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MaterialController/Create
        public ActionResult Create()
        {
            return View(new MaterialVM());
        }

        // POST: MaterialController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MaterialVM materialVM)
        {
            if (_materialRepository.MaterialNameExists(materialVM.Name))
            {
                ModelState.AddModelError("Name", "A material with this name already exists.");
                return View(materialVM);
            }
            try
            {
                var materialEntity = _mapper.Map<Material>(materialVM);
               
                _materialRepository.Add(materialEntity);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
                return View(materialVM);
            }
        }

        // GET: MaterialController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MaterialController/Edit/5
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

        // GET: MaterialController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MaterialController/Delete/5
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
