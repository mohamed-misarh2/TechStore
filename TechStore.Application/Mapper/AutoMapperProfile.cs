using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Dtos.Category;
using TechStore.Models;

namespace TechStore.Application.Mapper
{
    internal class AutoMapperProfile :Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<CreateOrUpdateCategory,Category>().ReverseMap();
            CreateMap<GetAllCategory,Category>().ReverseMap();
        }
    }
}
