using AutoMapper;
using BL.IServices;
using BL.Services;
using StudyMaterialOrganiser.ViewModels;

namespace StudyMaterialOrganiser.Utilities
{
    public class AssignTags
    {
        private readonly ITagService _tagservice;
        private readonly IMapper _mapper;

        public AssignTags(ITagService tagservice, IMapper mapper)
        {
            _tagservice = tagservice;
            _mapper = mapper;
        }

        public List<TagVM> AssignTag()
        {
            var tags = _tagservice.GetAll().ToList();
            var tagsVM = _mapper.Map<List<TagVM>>(tags);

            return tagsVM;
        }

    }
}
