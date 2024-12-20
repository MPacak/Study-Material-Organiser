using AutoMapper;
using DAL.IRepositories;
using BL.IServices;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Models;
using DAL.Models;

namespace BL.Services
{
    public class MaterialTagService : IMaterialTagService 
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public MaterialTagService(IUnitOfWork unitOfWork, IConfiguration configuration, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _mapper = mapper;
        }

        public void Create(MaterialTagDto materialTagDto)
        {
            var materialTag = _mapper.Map<MaterialTag>(materialTagDto);
            _unitOfWork.MaterialTag.Add(materialTag);
            _unitOfWork.Save();
        }
        public void DeleteByMaterialId(int materialId)
        {
            var materialTags = _unitOfWork.MaterialTag
                .GetAll()
                .Where(mt => mt.MaterialId == materialId);

            foreach (var tag in materialTags)
            {
                _unitOfWork.MaterialTag.Delete(tag);
            }
            _unitOfWork.Save();
        }

        public IEnumerable<MaterialTagDto> GetByMaterialId(int materialId)
        {
            var materialTags = _unitOfWork.MaterialTag
                .GetAll()
                .Where(mt => mt.MaterialId == materialId)
                .ToList();

            return _mapper.Map<IEnumerable<MaterialTagDto>>(materialTags);
        }

        public void Update(int materialId, List<int> TagIds)
        {
            var existingMaterial = _unitOfWork.Material
        .GetAll(
            filter: m => m.Idmaterial == materialId,
            includeProperties: "MaterialTags")
        .FirstOrDefault();

            if (existingMaterial == null)
                throw new InvalidOperationException($"Material with ID {materialId} not found");

            
            foreach (var materialTag in existingMaterial.MaterialTags.ToList())
            {
                _unitOfWork.MaterialTag.Delete(materialTag);
            }

            
            foreach (var tagId in TagIds)
            {
                var newMaterialTag = new MaterialTag
                {
                    MaterialId = materialId,
                    TagId = tagId
                };
                _unitOfWork.MaterialTag.Add(newMaterialTag);
            }

            _unitOfWork.Save();
        }
    }
}
