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

            CreateMap<TagVM, TagDto>();
            CreateMap<TagDto,TagVM>();

            CreateMap<MaterialDto, MaterialVM>().ReverseMap();

            CreateMap<UserDto, UserEditVM>().ReverseMap();
            CreateMap<UserDto, ProfileEditVM>().ReverseMap();
            CreateMap<Log, LogDto>().ReverseMap();
        }
    }
}
