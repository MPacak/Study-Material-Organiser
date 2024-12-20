﻿using AutoMapper;
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

        public void Delete(MaterialDto materialDto)
        {
            var material = _unitOfWork.Material
      .GetAll(
          filter: m => m.Idmaterial == materialDto.Id,
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
          filter: m => m.Idmaterial == data.Id,
          includeProperties: "MaterialTags")
      .FirstOrDefault();

            if (existingMaterial == null)
                throw new InvalidOperationException($"Material with ID {data.Id} not found");


            _mapper.Map(data, existingMaterial);
            _unitOfWork.Save();

            if (data.TagIds != null)
            {
                //ovdje bi trebalo samo pozvati update na material tag i ubaciti tagsid jer imamo ovu istu formulu tamo
                var tagsToRemove = existingMaterial.MaterialTags
                    .Where(mt => !data.TagIds.Contains(mt.TagId))
                    .ToList();

                foreach (var tagToRemove in tagsToRemove)
                {
                    _unitOfWork.MaterialTag.Delete(tagToRemove);
                }


                var existingTagIds = existingMaterial.MaterialTags.Select(mt => mt.TagId);
                var newTagIds = data.TagIds.Except(existingTagIds);

                foreach (var tagId in newTagIds)
                {
                    var newMaterialTag = new MaterialTag
                    {
                        MaterialId = existingMaterial.Idmaterial,
                        TagId = tagId
                    };
                    _unitOfWork.MaterialTag.Add(newMaterialTag);
                }
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
