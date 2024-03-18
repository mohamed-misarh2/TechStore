using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Dtos.AccountDtos;
using TechStore.Dtos;
using TechStore.Models;
using TechStore.Dtos.UserDTO;
using TechStore.Dtos.CategoryDtos;
using TechStore.Dtos.AccountDtos;
using Microsoft.AspNetCore.Identity;
using TechStore.Dtos.ReviewDtos;


namespace TechStore.Application.Mapper
{
    internal class AutoMapperProfile :Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<UpdateUserDTO, TechUser>().ReverseMap();
            CreateMap<GetAllUserDTO, TechUser>().ReverseMap();
            CreateMap<LoginDto, TechUser>().ReverseMap();
            CreateMap<RegisterDto, TechUser>().ReverseMap();
            CreateMap<CreateOrUpdateCategory,Category>().ReverseMap();
            CreateMap<GetAllCategory,Category>().ReverseMap();
            CreateMap<CreateOrUpdateReviewDto, Review>().ReverseMap();
            CreateMap<GetAllReviewDto, Review>().ReverseMap();

            CreateMap<RegisterDto, TechUser>().ReverseMap();
            CreateMap<LoginDto, TechUser>().ReverseMap();
            CreateMap<UserDto, TechUser>().ReverseMap();
            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap<CreateOrUpdateReviewDto, Review>().ReverseMap();
            CreateMap<GetAllReviewDto, Review>().ReverseMap();
           
        }
    }
}
