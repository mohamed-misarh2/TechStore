using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Application.Contract;
using TechStore.Dtos.ProductDtos;
using TechStore.Dtos.ViewResult;
using TechStore.Models;

namespace TechStore.Application.Services
{
    public interface IProductService
    {
        Task<ResultView<ProductCategorySpecificationsListDto>> Create(CreateOrUpdateProductDtos productDto, List<ProductCategorySpecificationsDto> ProductCategorySpecificationsDto);

        Task<ResultView<GetProductSpecificationNameValueDtos>> GetOne(int id);

        Task<ResultView<ProductCategorySpecificationsListDto>> Update(CreateOrUpdateProductDtos productDto, List<ProductCategorySpecificationsDto> ProductCategorySpecificationsDto);

        Task<ResultView<ProductCategorySpecificationsListDto>> SoftDelete(int productId);

        Task<ResultView<ProductCategorySpecificationsListDto>> HardDelete(int productId);

        Task<ResultDataList<GetAllProductsDtos>> GetAllPagination(int ItemsPerPage, int PageNumber);














        //search
        Task<ResultDataList<CreateOrUpdateProductDtos>> SearchProduct(string Name, int ItemsPerPage, int PageNumber);

        Task<ResultDataList<CreateOrUpdateProductDtos>> SearchByBrand(string Brand, int ItemsPerPage, int PageNumber);



        //filter
        Task<ResultDataList<CreateOrUpdateProductDtos>> FilterProductsByCategory(int categoryId, int ItemsPerPage, int PageNumber);

        Task<ResultDataList<CreateOrUpdateProductDtos>> FiltertRelatedProducts(int productId, int ItemsPerPage, int PageNumber);

        //Task<ResultDataList<CreateOrUpdateProductDtos>> FilterProductsByPriceRange(decimal minPrice, decimal maxPrice, int ItemsPerPage, int PageNumber);

        Task<ResultDataList<CreateOrUpdateProductDtos>> FilterNewlyAddedProductsAsync(int count, int ItemsPerPage, int PageNumber);

        //Task<ResultDataList<CreateOrUpdateProductDtos>> FilterDiscountedProducts(int ItemsPerPage, int PageNumber);

        //Task<ResultDataList<GetAllProductsForUserDto>> GetAllPaginationForUser(int ItemsPerPage, int PageNumber);
    }
}
