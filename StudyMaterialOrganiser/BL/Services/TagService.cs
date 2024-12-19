using AutoMapper;
using BL.IServices;
using BL.Models;
using DAL.IRepositories;
using DAL.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class TagService : ITagService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public TagService(IUnitOfWork unitOfWork, IConfiguration configuration, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _mapper = mapper;
        }

        public IEnumerable<TagDto> GetAll()
        {
            var tags = _unitOfWork.Tag.GetAll();
            return _mapper.Map<IEnumerable<TagDto>>(tags);
        }

        public TagDto GetById(int id)
        {
            var tag = _unitOfWork.Tag.GetFirstOrDefault(t => t.Idtag == id);
            if (tag == null)
            {
                throw new KeyNotFoundException($"Tag with ID {id} not found.");
            }
            return _mapper.Map<TagDto>(tag);
        }

        public TagDto GetByName(string name)
        {
            var tag = _unitOfWork.Tag.GetFirstOrDefault(t => t.TagName == name);
            if (tag == null)
            {
                throw new KeyNotFoundException($"Tag with name '{name}' not found.");
            }
            return _mapper.Map<TagDto>(tag);
        }

        public void Create(TagDto tagDto)
        {
            // Validate name is not empty
            if (string.IsNullOrWhiteSpace(tagDto.Name))
            {
                throw new ArgumentException("Tag name cannot be empty.");
            }

            // Check if tag with same name already exists
            var existingTag = _unitOfWork.Tag.GetFirstOrDefault(t => t.TagName == tagDto.Name);
            if (existingTag != null)
            {
                throw new InvalidOperationException($"Tag with name '{tagDto.Name}' already exists.");
            }

            var tag = _mapper.Map<Tag>(tagDto);
            _unitOfWork.Tag.Add(tag);
            _unitOfWork.Save();
        }

        public void Update(TagDto tagDto)
        {
            
            if (string.IsNullOrWhiteSpace(tagDto.Name))
            {
                throw new ArgumentException("Tag name cannot be empty.");
            }

           
            var existingTag = _unitOfWork.Tag.GetFirstOrDefault(t => t.Idtag == tagDto.Id);
            if (existingTag == null)
            {
                throw new KeyNotFoundException($"Tag with ID {tagDto.Id} not found.");
            }

           
            var nameConflict = _unitOfWork.Tag.GetFirstOrDefault(t =>
                t.TagName == tagDto.Name);
            if (nameConflict != null)
            {
                throw new InvalidOperationException($"Tag with name '{tagDto.Name}' already exists.");
            }

            _mapper.Map(tagDto, existingTag);
            
            _unitOfWork.Save();
        }

        public void Delete(int id)
        {
            var tag = _unitOfWork.Tag.GetFirstOrDefault(t => t.Idtag == id);
            if (tag == null)
            {
                throw new KeyNotFoundException($"Tag with ID {id} not found.");
            }

            _unitOfWork.Tag.Delete(tag);
            _unitOfWork.Save();
        }

        public bool Exists(string name)
        {
            return _unitOfWork.Tag.GetFirstOrDefault(t => t.TagName == name) != null;
        }
    }
}

