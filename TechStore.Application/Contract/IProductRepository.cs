using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Dtos;
using TechStore.Dtos.ViewResult;
using TechStore.Models;

namespace TechStore.Application.Contract
{
    public interface IProductRepository:IRepository<Product,int>
    {
        Task<IQueryable<Product>> SearchProduct(string Name);
        Task<IQueryable<Product>> SearchByBrand(string Brand);
        Task<IQueryable<Product>> GetProductsByCategory(int categoryId);
        Task<IQueryable<Product>> GetRelatedProducts(Product product);//samecategory
        //Task<IQueryable<Product>> GetProductsByPriceRange(decimal minPrice, decimal maxPrice);
        Task<IQueryable<Product>> GetNewlyAddedProducts(int count);
        //Task<IQueryable<Product>> GetDiscountedProducts();
        
    }
}
