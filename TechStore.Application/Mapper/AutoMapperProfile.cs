using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Dtos;
using TechStore.Dtos.ProductDtos;
using TechStore.Models;
using TechStore.Dtos.UserDTO;
using TechStore.Dtos.CategoryDtos;
using Microsoft.AspNetCore.Identity;
using TechStore.Dtos.ReviewDtos;


namespace TechStore.Application.Mapper
{
    public class AutoMapperProfile :Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UpdateUserDTO, TechUser>().ReverseMap();
            CreateMap<GetAllUserDTO, TechUser>().ReverseMap();
            CreateMap<LoginDto, TechUser>().ReverseMap();
            CreateMap<RegisterDto, TechUser>().ReverseMap();



            CreateMap<CreateOrUpdateProductDtos, Product>().ReverseMap();
            CreateMap<SpecificationsDto, Specification>().ReverseMap();


            CreateMap<ProductCategorySpecificationsDto, ProductCategorySpecifications>().ReverseMap();

            //productitem

            CreateMap<GetAllProductsDtos, Product>().ReverseMap();
            CreateMap<GetAllProductsForUserDto, Product>().ReverseMap();

            CreateMap<UserDto, TechUser>().ReverseMap();
            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap <CreateOrUpdateReviewDto, Review>().ReverseMap();
            CreateMap<GetAllReviewDto, Review>().ReverseMap();


            

        }
    }
}
