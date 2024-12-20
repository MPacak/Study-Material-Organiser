using AutoMapper;
using BL.Models;
using DAL.Models;
using ViewModels;
using StudyMaterialOrganiser.ViewModels;


namespace StudyMaterialOrganiser.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<TagVM, TagDto>().ReverseMap();

            CreateMap<MaterialDto, MaterialVM>()
                .ForMember(dest => dest.SelectedTagIds, opt => opt.MapFrom(src => src.TagIds))
               .ForMember(dest => dest.File, opt => opt.Ignore());

            CreateMap<MaterialVM, MaterialDto>()
                 .ForMember(dest => dest.TagIds, opt => opt.MapFrom(src => src.SelectedTagIds));


            CreateMap<UserDto, UserEditVM>().ReverseMap();
            CreateMap<UserDto, ProfileEditVM>().ReverseMap();
            CreateMap<Log, LogDto>().ReverseMap();
        }
    }
}
