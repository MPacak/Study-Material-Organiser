using AutoMapper;
using BL.Models;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.AutoMaperProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DAL.Models.User, Models.UserDto>().ReverseMap();
            CreateMap<DAL.Models.Log, Models.LogDto>().ReverseMap();
            CreateMap<Material, MaterialDto>()
               .ForMember(dest => dest.TagIds, opt => opt.MapFrom(src => src.MaterialTags.Select(mt => mt.Tag.Idtag)))
               .ForMember(dest => dest.TagNames, opt => opt.MapFrom(src => src.MaterialTags.Select(mt => mt.Tag.TagName)));
            CreateMap<MaterialDto, Material>()
                .ForMember(dest => dest.MaterialTags, opt => opt.Ignore());
            CreateMap<Tag, TagDto>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Idtag))
           .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.TagName))
           .ForMember(dest => dest.Groups, opt => opt.Ignore());
            CreateMap<TagDto, Tag>()
                .ForMember(dest => dest.Idtag, opt => opt.Ignore())
                .ForMember(dest => dest.TagName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Groups, opt => opt.Ignore())
                .ForMember(dest => dest.MaterialTags, opt => opt.Ignore());
        }
    }
}
