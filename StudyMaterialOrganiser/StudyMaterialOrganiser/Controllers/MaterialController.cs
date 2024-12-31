﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DAL.IRepositories;
using AutoMapper;
using StudyMaterialOrganiser.ViewModels;
using DAL.Models;
using BL.Models;
using BL.IServices;
using Microsoft.AspNetCore.Hosting;

namespace StudyMaterialOrganiser.Controllers
{
    public class MaterialController : Controller
    {
 
        private readonly IMaterialService _materialService;
        private readonly ITagService _tagService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MaterialController(IMaterialService materialService, ITagService tagService, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _materialService = materialService;
            _tagService = tagService;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: MaterialController
        public ActionResult Index()
        {

            return View();
        }
       /* public ActionResult List()
        {
            var materials = _materialService.GetAll();
            var viewModels = _mapper.Map<List<MaterialVM>>(materials);

            return View(viewModels);
        }*/
        public ActionResult List(string? query, int? fileType, List<int>? tagIds, int page = 1, int pageSize = 10)
        {
            var materialsDto = _materialService.GetAll();

            var materials = materialsDto.Select(x => _mapper.Map<MaterialVM>(x));

            if (!string.IsNullOrEmpty(query))
            {
                materials = materials.Where(m =>
                    m.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    (m.Description != null && m.Description.Contains(query, StringComparison.OrdinalIgnoreCase)));
            }

            if (fileType.HasValue)
            {
                materials = materials.Where(m => m.FolderTypeId == fileType.Value);
            }

            if (tagIds != null && tagIds.Any())
            {
                materials = materials.Where(m =>
                   //  m.SelectedTagIds.Any(tagId => tagIds.Contains(tagId)));
                   m.TagIds.Any(tagId => tagIds.Contains(tagId)));
            }

            var totalItems = materials.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var paginatedMaterials = materials
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

         

            var searchVM = new MaterialSearchVM
            {
                Materials = paginatedMaterials,
                CurrentPage = page,
                TotalPages = totalPages,
                PageSize = pageSize,
                TotalItems = totalItems,
                Query = query,
                FileType = fileType,
                TagIds = tagIds,
                AvailableTags = AssignTags()
            };

            return View(searchVM);
        }

        // GET: MaterialController/Details/5
        public ActionResult Details(int id)
        {
            var material = _materialService.GetMaterialById(id);

            if (material == null)
            {
                return NotFound(); 
            }
            var materialVM = _mapper.Map<MaterialVM>(material);
          

            return View(materialVM);
        }

        // GET: MaterialController/Create
        public ActionResult Create()
        {

            var viewModel = new MaterialVM
            {
                AvailableTags = AssignTags()
            };
            return View(viewModel);
        }

        private List<TagVM> AssignTags()
        {
            var tags = _tagService.GetAll().ToList();
            var tagsVM = _mapper.Map<List<TagVM>>(tags);
            
            return tagsVM;
        }

        // POST: MaterialController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(MaterialVM materialVM)
        {
            //S
            materialVM.AvailableTags = AssignTags();
            try
            {
                if (materialVM.File != null)
                {
                    
                    var fileType = FileTypeExtensions.GetFileTypeFromExtension(materialVM.File.FileName);
                    if (!fileType.HasValue)
                    {
                        ModelState.AddModelError("File", "Invalid file type");
                        return View(materialVM);
                    }

                
                    var fileName = Path.GetRandomFileName() + Path.GetExtension(materialVM.File.FileName);
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    Directory.CreateDirectory(uploadsFolder);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await materialVM.File.CopyToAsync(stream);
                    }

                    materialVM.Link = GenerateShareableLink(materialVM.Idmaterial);
                    materialVM.FilePath = fileName;
                    materialVM.FolderTypeId = (int)fileType.Value;
                    var materialDto = _mapper.Map<MaterialDto>(materialVM);

                    _materialService.Create(materialVM);
                    //_materialService.Create(materialDto);
                    return View("Confirmation", new ConfirmationVM
                    {
                        Message = "Material was successfully created.",
                        ActionName = nameof(List),
                        ControllerName = "Material",
                        RedirectSeconds = 3
                    });
                }

                ModelState.AddModelError("File", "No file was uploaded");
                return View(materialVM);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("Name", "A material with this name already exists.");
                return View(materialVM);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
                return View(materialVM);
            }
        }

        private string GenerateShareableLink(int materialId)
        {
            var request = HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";
            
            return $"{baseUrl}/Material/Access/{materialId}";
        }

        // GET: MaterialController/Edit/5
        // GET: MaterialController/Edit/5
        public ActionResult Edit(int id)
        {
            var material = _materialService.GetMaterialById(id);
            if (material == null)
            {
                return NotFound();
            }

            var materialVM = _mapper.Map<MaterialVM>(material);

            materialVM.AvailableTags = AssignTags();

           // materialVM.SelectedTagIds = material.TagIds;

            return View(materialVM);
        }

        // POST: MaterialController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, MaterialVM materialVM)
        {
            try
            {
                if (id != materialVM.Idmaterial)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    var existingMaterial = _materialService.GetMaterialById(id);
                    if (existingMaterial == null)
                    {
                        return NotFound();
                    }

                    if (materialVM.File != null)
                    {
                        var fileType = FileTypeExtensions.GetFileTypeFromExtension(materialVM.File.FileName);
                        if (!fileType.HasValue)
                        {
                            ModelState.AddModelError("File", "Invalid file type");
                            materialVM.AvailableTags = _mapper.Map<List<TagVM>>(_tagService.GetAll().ToList());
                            return View(materialVM);
                        }

                        var fileName = Path.GetRandomFileName() + Path.GetExtension(materialVM.File.FileName);
                        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                        var filePath = Path.Combine(uploadsFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await materialVM.File.CopyToAsync(stream);
                        }

                        if (!string.IsNullOrEmpty(existingMaterial.FilePath))
                        {
                            var oldFilePath = Path.Combine(uploadsFolder, existingMaterial.FilePath);
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        materialVM.FilePath = fileName;
                        materialVM.FolderTypeId = (int)fileType.Value;
                    }
                    else
                    {

                        materialVM.FilePath = existingMaterial.FilePath;
                        materialVM.FolderTypeId = existingMaterial.FolderTypeId;
                    }

                    materialVM.Link = existingMaterial.Link;

                    var materialDto = _mapper.Map<MaterialDto>(materialVM);
                    _materialService.Update(materialDto);

                    return View("Confirmation", new ConfirmationVM
                    {
                        Message = "Material was successfully updated.",
                        ActionName = nameof(List),
                        ControllerName = "Material",
                        RedirectSeconds = 3
                    });
                }

                materialVM.AvailableTags = AssignTags();
                return View(materialVM);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("Name", "A material with this name already exists.");
                return View(materialVM);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
                materialVM.AvailableTags = AssignTags();
                return View(materialVM);
            }
        }

        // GET: MaterialController/Delete/5
        public ActionResult Delete(int id)
        {
            var materialVm = _mapper.Map<MaterialVM>(_materialService.GetMaterialById(id));
            return View(materialVm);
        }

        // POST: MaterialController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, MaterialVM materialVM)
        {
            try
            {
                var material = _mapper.Map<MaterialDto>(materialVM);
                _materialService.Delete(material);
                return View("Confirmation", new ConfirmationVM
                {
                    Message = "Material was successfully deleted.",
                    ActionName = nameof(List),
                    ControllerName = "Material",
                    RedirectSeconds = 3
                });
            }
              
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = "Deletion was unsuccessful. Please try again.";
                return View(materialVM);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
                return View(materialVM);
            }
        }
        public ActionResult Access(int id)
        {
            try
            {
                var material = _materialService.GetMaterialById(id);
                if (material == null)
                {
                    return NotFound();
                }

                var viewModel = _mapper.Map<MaterialVM>(material);
                return View("Details", viewModel);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
    }
}
