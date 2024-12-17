using AutoMapper;
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
        }
    }
}
