using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Dtos.AccountDtos;
using TechStore.Dtos.UserDTO;
using TechStore.Models;


namespace TechStore.Application.Mapper
{
    internal class AutoMapperProfile :Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<CreateOrUpdateUserDTO, TechUser>().ReverseMap();
            CreateMap<GetAllUserDTO, TechUser>().ReverseMap();
            CreateMap<LoginDto, TechUser>().ReverseMap();
            CreateMap<RegisterDto, TechUser>().ReverseMap();
        }
    }
}
