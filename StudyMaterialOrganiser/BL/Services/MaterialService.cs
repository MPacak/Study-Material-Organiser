using AutoMapper;
using BL.IServices;
using BL.Models;
using DAL.IRepositories;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class MaterialService: IMaterialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IMaterialTagService _materialTagService;

        public MaterialService(IUnitOfWork unitOfWork, IConfiguration configuration, IMapper mapper, IMaterialTagService materialTagService)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _mapper = mapper;
            _materialTagService = materialTagService;
        }

        public MaterialDto Create(MaterialDto materialDto)
        {
            Console.WriteLine("service started");
            Console.WriteLine($"Creating Material: {materialDto.Name}");
            Console.WriteLine($"FilePath: {materialDto.FilePath}");
            Console.WriteLine($"FolderTypeId: {materialDto.FolderTypeId}");
            Console.WriteLine($"Link: {materialDto.Link}");
            Console.WriteLine($"TagIds: {string.Join(", ", materialDto.TagIds)}");
            var material = _mapper.Map<Material>(materialDto);
            var checkMaterial= _unitOfWork.Material.GetFirstOrDefault(u => u.Name == material.Name);
            if (checkMaterial != null)
            {
                throw new InvalidOperationException();
            }

            _unitOfWork.Material.Add(material);
            Console.WriteLine("Material added to UnitOfWork.");
            materialDto.Idmaterial = material.Idmaterial;
            Console.WriteLine($"Creating Material: {materialDto.Idmaterial}");
            _unitOfWork.Save();
            if (materialDto.TagIds != null && materialDto.TagIds.Any())
            {
                _materialTagService.Create(material.Idmaterial, materialDto.TagIds);
                _unitOfWork.Save();
            }
            Console.WriteLine($"it did not pass materialtagservice");
            return materialDto;
        }

        public void Delete(MaterialDto materialDto)
        {
            var material = _unitOfWork.Material
      .GetAll(
          filter: m => m.Idmaterial == materialDto.Idmaterial,
          includeProperties: "MaterialTags")
      .FirstOrDefault();

            if (material != null)
            {
                foreach (var materialTag in material.MaterialTags.ToList())
                {
                    _unitOfWork.MaterialTag.Delete(materialTag);
                }

                _unitOfWork.Material.Delete(material);

                _unitOfWork.Save();
            } else
            {
                throw new InvalidOperationException("The material was not found");
            }
        }

        public IEnumerable<MaterialDto> GetAll()
        {
            var materials = _unitOfWork.Material
        .GetAll(includeProperties: "MaterialTags,MaterialTags.Tag");
            return _mapper.Map<IEnumerable<MaterialDto>>(materials);
        }


        public void Update(MaterialDto data)
        {
            var existingMaterial = _unitOfWork.Material
      .GetAll(
          filter: m => m.Idmaterial == data.Idmaterial,
          includeProperties: "MaterialTags")
      .FirstOrDefault();

            if (existingMaterial == null)
                throw new InvalidOperationException($"Material with ID {data.Idmaterial} not found");


            _mapper.Map(data, existingMaterial);
            _unitOfWork.Save();

            if (data.TagIds != null)
            {
                _materialTagService.Update(data.Idmaterial, data.TagIds);
                _unitOfWork.Save();
            }
        }

       public MaterialDto? GetMaterialById(int materialId)
        {
            var material = _unitOfWork.Material
       .GetAll(
           filter: m => m.Idmaterial == materialId,
           includeProperties: "MaterialTags,MaterialTags.Tag")
       .FirstOrDefault();
            return material != null ? _mapper.Map<MaterialDto>(material) : null;
        }

        public MaterialDto? GetMaterialByName(string materialName)
        {
            var material = _unitOfWork.Material
                .GetAll(
            filter: m => m.Name == materialName,
            includeProperties: "MaterialTags,MaterialTags.Tag")
        .FirstOrDefault();
            return material != null ? _mapper.Map<MaterialDto>(material) : null;
        }

    }
}
