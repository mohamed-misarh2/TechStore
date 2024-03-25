using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Dtos;
using TechStore.Dtos.ProductDtos;
using TechStore.Dtos.ViewResult;
using TechStore.Models;

namespace TechStore.Application.Contract
{
    public interface IProductRepository:IRepository<Product,int>
    {
        Task DetachEntityAsync(Product entity);
        
        //search
        Task<IQueryable<Product>> SearchProduct(string Name);
        Task<IQueryable<Product>> SearchByBrand(string Brand);

        //filter
        Task<IQueryable<Product>> GetProductsByCategory(int categoryId);
        Task<IQueryable<Product>> GetRelatedProducts(Product product);
        Task<IQueryable<Product>> GetProductsByPriceRange(decimal minPrice, decimal maxPrice);
        Task<IQueryable<Product>> GetNewlyAddedProducts(int count);
        Task<IQueryable<Product>> GetDiscountedProducts();
        Task<IQueryable<Product>> GetProductsByWarranty(string Warranty);

        //sort
        Task<IQueryable<Product>> GetProductsByDescending();
        Task<IQueryable<Product>> GetProductsByAscending();
    }
}
