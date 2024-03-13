using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Dtos.AccountDtos;
using TechStore.Dtos;
using TechStore.Dtos;
using TechStore.Dtos.ProductDtos;
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
            CreateMap<CreateOrUpdateCategory,Category>().ReverseMap();
            CreateMap<GetAllCategory,Category>().ReverseMap();
            CreateMap<CreateOrUpdateReviewDto, Review>().ReverseMap();
            CreateMap<GetAllReviewDto, Review>().ReverseMap();

        public AutoMapperProfile() {
            CreateMap<CreateOrUpdateProductDtos, Product>().ReverseMap();
            //productitem
            CreateMap<LabtopItemDtos, ProductItem>().ReverseMap();
            CreateMap<MobileAndTabletItemDtos, ProductItem>().ReverseMap();
            CreateMap<ScreenItemDtos, ProductItem>().ReverseMap();
            CreateMap<SmartwatchItemDtos, ProductItem>().ReverseMap();
            CreateMap<GetAllProductsForAdminDto, Product>().ReverseMap();
            CreateMap<GetAllProductsForUserDto, Product>().ReverseMap();
        }
    }
}
