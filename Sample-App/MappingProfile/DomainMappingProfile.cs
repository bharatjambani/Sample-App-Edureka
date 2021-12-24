using AutoMapper;
using Sample_App.Models;
using Sample_App.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample_App.MappingProfile
{
    public class DomainMappingProfile : Profile
    {
        public DomainMappingProfile()
        {
            CreateMap<Product, ProductEditModel>().ReverseMap();
            //CreateMap<Product, ProductAddModel>().ReverseMap();
            //CreateMap<RegisterViewModel, ApplicationUser>().ReverseMap();

            CreateMap<RegisterViewModel, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
        }
    }
}
