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

        public MaterialService(IUnitOfWork unitOfWork, IConfiguration configuration, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _mapper = mapper;
        }

        public void Create(MaterialDto materialDto)
        {
            var material = _mapper.Map<Material>(materialDto);
            var checkMaterial= _unitOfWork.Material.GetFirstOrDefault(u => u.Name == material.Name);
            if (checkMaterial != null)
            {
                throw new InvalidOperationException();
            }

            _unitOfWork.Material.Add(material);
            _unitOfWork.Save();
            if (materialDto.TagIds != null && materialDto.TagIds.Any())
            {
                foreach (var tagId in materialDto.TagIds)
                {
                    var materialTag = new MaterialTag
                    {
                        MaterialId = material.Idmaterial,
                        TagId = tagId
                    };
                    _unitOfWork.MaterialTag.Add(materialTag);
                }
                _unitOfWork.Save();
            }
        }

        public void Delete(MaterialDto materialdto)
        {
            var material = _mapper.Map<Material>(materialdto);
            _unitOfWork.Material.Delete(material);
            _unitOfWork.Save();
        }

        public IEnumerable<MaterialDto> GetAll()
        {
            var materials = _unitOfWork.Material
        .GetAll(includeProperties: "MaterialTags,MaterialTags.Tag");
            return _mapper.Map<IEnumerable<MaterialDto>>(materials);
        }


        public void Update(int id, MaterialDto data)
        {
            
        }

        MaterialDto? IMaterialService.GetMaterialById(int materialId)
        {
            var material = _unitOfWork.Material
       .GetAll(
           filter: m => m.Idmaterial == materialId,
           includeProperties: "MaterialTags,MaterialTags.Tag")
       .FirstOrDefault();
            return material != null ? _mapper.Map<MaterialDto>(material) : null;
        }

        MaterialDto? IMaterialService.GetMaterialByName(string materialName)
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
