using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Dtos;
using TechStore.Models;
using TechStore.Models;
using TechStore.Models;
using TechStore.Dtos.UserDTO;
using TechStore.Models;


namespace TechStore.Application.Mapper
{
    internal class AutoMapperProfile :Profile
    {
        public AutoMapperProfile() {

            CreateMap<CreateOrUpdateReviewDto, Review>().ReverseMap();
            CreateMap<GetAllReviewDto, Review>().ReverseMap();
           






        }
     
        public AutoMapperProfile() 
        {
        }
     
        public AutoMapperProfile() 
        {
            CreateMap<CreateOrUpdateUserDTO, TechUser>().ReverseMap();
            CreateMap<GetAllUserDTO, TechUser>().ReverseMap();
        }
    }
}
