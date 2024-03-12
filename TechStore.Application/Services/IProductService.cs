using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Dtos;
using TechStore.Dtos.ViewResult;

namespace TechStore.Application.Services
{
    public interface IProductService
    {
        Task<ResultView<ProductDto>> Create(ProductDto productDto);
        Task<ResultView<ProductDto>> Update(ProductDto productDto);
        Task<ResultView<ProductDto>> SoftDelete(ProductDto productDto);
        Task<ResultDataList<ProductDto>> SearchProduct(string Name);
        Task<ResultDataList<ProductDto>> SearchByBrand(string Brand);
        Task<ResultDataList<ProductDto>> GetProductsByCategory(int categoryId);
        Task<ResultDataList<ProductDto>> GetRelatedProducts(int productId);//samecategory
        Task<ResultDataList<ProductDto>> GetProductsByPriceRange(decimal minPrice, decimal maxPrice);
        Task<ResultDataList<ProductDto>> GetNewlyAddedProductsAsync(int count);
        Task<ResultDataList<ProductDto>> GetDiscountedProducts();
        Task<ResultDataList<GetAllProductsForAdminDto>> GetAllPaginationForAdmin(int items,int PageNumber);
        //Task<ResultDataList<GetAllProductsForUserDto>> GetAllPaginationForUser(int items, int PageNumber);
        Task<ResultView<ProductDto>> GetOne(int id);
    }
}
