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
    }
}
