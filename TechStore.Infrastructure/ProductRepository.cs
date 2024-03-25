using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Application.Contract;
using TechStore.Context;
using TechStore.Dtos;
using TechStore.Dtos.ProductDtos;
using TechStore.Dtos.ViewResult;
using TechStore.Models;

namespace TechStore.Infrastructure
{
    public class ProductRepository : Repository<Product, int>, IProductRepository
    {

        public ProductRepository(TechStoreContext techStoreContext) : base(techStoreContext) { }

        public async Task<IQueryable<Product>> GetDiscountedProducts()
        {
            return await Task.FromResult(_entities.Where(p => p.DiscountedPrice < p.Price));
        }

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

        public async Task<IQueryable<Product>> GetProductsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            return await Task.FromResult(_entities.Where(p => p.Price >= minPrice && p.Price <= maxPrice));
        }

        public Task<IQueryable<Product>> GetRelatedProducts(Product product)
        {
            return Task.FromResult( _entities.Where(p => p.CategoryId == product.CategoryId || p.Id != product.Id));
        }

        public Task<IQueryable<Product>> SearchByBrand(string Brand)
        {
            return Task.FromResult(_entities.Where(p => p.Brand.Contains(Brand)));
        }

        public Task<IQueryable<Product>> SearchProduct(string Name)
        {          
            return Task.FromResult(_entities.Where(p => p.ModelName.Contains(Name)||
                                                   p.Description.Contains(Name)));
        }

        public Task<IQueryable<Product>> GetProductsByWarranty(string Warranty)
        {
            return Task.FromResult(_entities.Where(p => p.Warranty == Warranty));
        }

        public async Task<IQueryable<Product>> GetProductsByDescending()
        {
            return await Task.FromResult(_entities.OrderByDescending(p => p.Price));
        }

        public async Task<IQueryable<Product>> GetProductsByAscending()
        {
            return await Task.FromResult(_entities.OrderBy(p=>p.Price));
        }

        public async Task<IQueryable<Product>> FilterProducts(FillterProductsDtos criteria)
        {
            IQueryable<Product> query = _entities;

            // Apply filters based on criteria
            if (!string.IsNullOrEmpty(criteria.Brand))
            {
                query = query.Where(p => p.Brand.Contains(criteria.Brand));
            } 
            if (!string.IsNullOrEmpty(criteria.Warranty))
            {
                query = query.Where(p => p.Warranty.Contains(criteria.Warranty));
            }

            if (criteria.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= criteria.MinPrice.Value);
            }

            if (criteria.DiscountValue.HasValue)
            {
                query = query.Where(p => p.DiscountValue == criteria.DiscountValue.Value);
            }

            if (criteria.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= criteria.MaxPrice.Value);
            }

            // You can add more filters as needed...

            return await Task.FromResult(query);
        }
    }
}
