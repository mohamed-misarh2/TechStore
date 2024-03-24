using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Application.Contract;
using TechStore.Context;
using TechStore.Dtos;
using TechStore.Dtos.ViewResult;
using TechStore.Models;

namespace TechStore.Infrastructure
{
    public class ProductRepository : Repository<Product, int>, IProductRepository
    {

        public ProductRepository(TechStoreContext techStoreContext) : base(techStoreContext) { }

        //public async Task<IQueryable<Product>> GetDiscountedProducts()
        //{
        //    return await Task.FromResult(_entities.Include(p=>p.ProductItem.Select(p=>p.DiscountPrice < p.Price)));
        //}

        public async Task DetachEntityAsync(Product entity)
        {
            if (_context.Entry(entity).State != EntityState.Detached)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }
        }

        public Task<IQueryable<Product>> GetNewlyAddedProducts(int count)
        {
            return Task.FromResult(_entities.OrderByDescending(p => p.DateAdded).Take(count));
        }

        public Task<IQueryable<Product>> GetProductsByCategory(int categoryId)
        {
            return Task.FromResult(_entities.Where(p => p.CategoryId == categoryId));
        }

        //public async Task<IQueryable<Product>> GetProductsByPriceRange(decimal minPrice, decimal maxPrice)
        //{
        //    return await Task.FromResult(_entities.Include(p => p.ProductItem.Select(p => p.Price >= minPrice && p.Price <= maxPrice)));
        //}

        public Task<IQueryable<Product>> GetRelatedProducts(Product product)
        {
            return Task.FromResult( _entities.Where(p => p.CategoryId == product.CategoryId || p.Id != product.Id));
        }

        public Task<IQueryable<Product>> SearchByBrand(string Brand)
        {
            return Task.FromResult(_entities.Where(p => p.Brand.Contains(Brand)));// contains || ==
        }

        public Task<IQueryable<Product>> SearchProduct(string Name)
        {          
            return Task.FromResult(_entities.Where(p => p.ModelName.Contains(Name)||
                                                   p.Description.Contains(Name)));
        }

        
    }
}
