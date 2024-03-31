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
using TechStore.Dtos.AccountDtos;
<<<<<<< HEAD
=======
using TechStore.Dtos.OrderDtos;
>>>>>>> origin/master


namespace TechStore.Application.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UpdateUserDTO, TechUser>().ReverseMap();
            CreateMap<GetAllUserDTO, TechUser>().ReverseMap();
            CreateMap<LoginDto, TechUser>().ReverseMap();
            CreateMap<RegisterDto, TechUser>().ReverseMap();
            CreateMap<RoleDto, IdentityRole>().ReverseMap();



            CreateMap<CreateOrUpdateProductDtos, Product>().ReverseMap();
            CreateMap<SpecificationsDto, Specification>().ReverseMap();


            CreateMap<ProductCategorySpecificationsDto, ProductCategorySpecifications>().ReverseMap();


            CreateMap<GetAllProductsDtos, Product>().ReverseMap();
            CreateMap<GetAllProductsForUserDto, Product>().ReverseMap();

            CreateMap<UserDto, TechUser>().ReverseMap();
            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap<CreateOrUpdateReviewDto, Review>().ReverseMap();
            CreateMap<GetAllReviewDto, Review>().ReverseMap();

            CreateMap<OrderDto, Order>().ReverseMap();
            CreateMap<GetAllOrderDto, Order>().ReverseMap();
            CreateMap<OrderItemDto, OrderItem>().ReverseMap();
            CreateMap<GetAllOrderItemDto, OrderItem>().ReverseMap();


        }
    }
}