using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Dtos;
using TechStore.Models;

namespace TechStore.Application.Mapper
{
    internal class AutoMapperProfile :Profile
    {
        public AutoMapperProfile() {
            CreateMap<ProductDto,Product>().ReverseMap();
            CreateMap<GetAllProductsForAdminDto, Product>().ReverseMap();
            CreateMap<GetAllProductsForUserDto, Product>().ReverseMap();
        }
    }
}
