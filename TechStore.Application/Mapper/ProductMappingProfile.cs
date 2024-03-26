using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Models;

namespace TechStore.Application.Mapper
{
    public class ProductMappingProfile:Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<IFormFile, Image>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FileName));
        }
    }
}
