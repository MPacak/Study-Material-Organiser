using AutoMapper;
using BL.IServices;
using BL.Models;
using DAL.IRepositories;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class StudyGroupService : IStudyGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StudyGroupService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public void Add(StudyGroupDto studyGroupDto)
        {
            var studyGroup = _mapper.Map<StudyGroup>(studyGroupDto);
            _unitOfWork.StudyGroup.Add(studyGroup);
            _unitOfWork.Save();
        }

        public IEnumerable<StudyGroupDto> GetAll()
        {
            var studyGroups = _unitOfWork.StudyGroup.GetAll(includeProperties: "Tag");
            return _mapper.Map<IEnumerable<StudyGroupDto>>(studyGroups);
        }

        public StudyGroupDto? GetGroupById(int id)
        {
            var studyGroup = _unitOfWork.StudyGroup.GetFirstOrDefault(sg => sg.Id == id, "Tag");
            return studyGroup == null ? null : _mapper.Map<StudyGroupDto>(studyGroup);
        }

        public void Remove(int id)
        {
            var studyGroup = _unitOfWork.StudyGroup.GetFirstOrDefault(sg => sg.Id == id);
            if (studyGroup != null)
            {
                //_unitOfWork.StudyGroup.Remove(studyGroup);
                _unitOfWork.Save();
            }
        }

        public void Remove(StudyGroupDto group)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, StudyGroupDto updatedDto)
        {
            var studyGroup = _unitOfWork.StudyGroup.GetFirstOrDefault(sg => sg.Id == id);
            if (studyGroup != null)
            {
                studyGroup.Name = updatedDto.Name;
                studyGroup.TagId = updatedDto.TagId;
                _unitOfWork.Save();
            }
        }

        StudyGroup? IStudyGroupService.GetGroupById(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
