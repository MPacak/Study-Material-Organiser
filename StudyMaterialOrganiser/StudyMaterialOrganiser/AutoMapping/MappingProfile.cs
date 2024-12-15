using AutoMapper;
using DAL.Models;
using StudyMaterialOrganiser.ViewModels;


namespace StudyMaterialOrganiser.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Tag to TagVM and vice versa
            CreateMap<Tag, TagVM>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Idtag))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.TagName))
                .ForMember(dest => dest.Groups, opt => opt.Ignore())
                .ForMember(dest => dest.MaterialTags, opt => opt.MapFrom(src => src.MaterialTags.Select(mt => mt.Material.Name)));

            CreateMap<TagVM, Tag>()
                .ForMember(dest => dest.Idtag, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.TagName, opt => opt.MapFrom(src => src.Name));

            CreateMap<Material, MaterialVM>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Idmaterial))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Link, opt => opt.MapFrom(src => src.Link))
                .ForMember(dest => dest.FilePath, opt => opt.MapFrom(src => src.FilePath))
                .ForMember(dest => dest.FolderTypeId, opt => opt.MapFrom(src => src.FolderTypeId))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.MaterialTags.Select(mt => mt.Tag.TagName)));

            CreateMap<MaterialVM, Material>()
                .ForMember(dest => dest.Idmaterial, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Link, opt => opt.MapFrom(src => src.Link))
                .ForMember(dest => dest.FilePath, opt => opt.MapFrom(src => src.FilePath))
                .ForMember(dest => dest.FolderTypeId, opt => opt.MapFrom(src => src.FolderTypeId));
        }
    }
}
