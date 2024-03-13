using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Application.Contract;
using TechStore.Dtos;
using TechStore.Dtos.ProductDtos;
using TechStore.Dtos.ViewResult;
using TechStore.Models;

namespace TechStore.Application.Services
{
    public interface IProductService
    {
        //Task<ResultView<CreateOrUpdateProductDtos>> Create(CreateOrUpdateProductDtos productDto);
        //Task<ResultView<CreateOrUpdateProductDtos>> Update(CreateOrUpdateProductDtos productDto);
        //Task<ResultView<CreateOrUpdateProductDtos>> SoftDelete(CreateOrUpdateProductDtos productDto);
        //Task<ResultDataList<CreateOrUpdateProductDtos>> SearchProduct(string Name);
        //Task<ResultDataList<CreateOrUpdateProductDtos>> SearchByBrand(string Brand);
        //Task<ResultDataList<CreateOrUpdateProductDtos>> GetProductsByCategory(int categoryId);
        //Task<ResultDataList<CreateOrUpdateProductDtos>> GetRelatedProducts(int productId);//samecategory
        //Task<ResultDataList<CreateOrUpdateProductDtos>> GetProductsByPriceRange(decimal minPrice, decimal maxPrice);
        //Task<ResultDataList<CreateOrUpdateProductDtos>> GetNewlyAddedProductsAsync(int count);
        //Task<ResultDataList<CreateOrUpdateProductDtos>> GetDiscountedProducts();
        //Task<ResultDataList<GetAllProductsForAdminDto>> GetAllPaginationForAdmin(int items,int PageNumber);
        ////Task<ResultDataList<GetAllProductsForUserDto>> GetAllPaginationForUser(int items, int PageNumber);
        //Task<ResultView<CreateOrUpdateProductDtos>> GetOne(int id);



        Task<ResultView<CreateOrUpdateProductDtos>> Create(CreateOrUpdateProductDtos productDto);

        Task<ResultView<CreateOrUpdateProductDtos>> GetOne(int id);

        Task<ResultView<CreateOrUpdateProductDtos>> Update(CreateOrUpdateProductDtos productDto);

        Task<ResultView<CreateOrUpdateProductDtos>> SoftDelete(CreateOrUpdateProductDtos productDto);

        Task<ResultDataList<GetAllProductsForAdminDto>> GetAllPaginationForAdmin(int ItemsPerPage, int PageNumber);

        Task<ResultDataList<CreateOrUpdateProductDtos>> SearchProduct(string Name, int ItemsPerPage, int PageNumber);

        Task<ResultDataList<CreateOrUpdateProductDtos>> SearchByBrand(string Brand, int ItemsPerPage, int PageNumber);

        Task<ResultDataList<CreateOrUpdateProductDtos>> GetProductsByCategory(int categoryId, int ItemsPerPage, int PageNumber);

        Task<ResultDataList<CreateOrUpdateProductDtos>> GetRelatedProducts(int productId, int ItemsPerPage, int PageNumber);

        Task<ResultDataList<CreateOrUpdateProductDtos>> GetProductsByPriceRange(decimal minPrice, decimal maxPrice, int ItemsPerPage, int PageNumber);

        Task<ResultDataList<CreateOrUpdateProductDtos>> GetNewlyAddedProductsAsync(int count, int ItemsPerPage, int PageNumber);

        Task<ResultDataList<CreateOrUpdateProductDtos>> GetDiscountedProducts(int ItemsPerPage, int PageNumber);

        Task<ResultDataList<GetAllProductsForUserDto>> GetAllPaginationForUser(int ItemsPerPage, int PageNumber);
    }
}
