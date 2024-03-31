using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Application.Contract;
using TechStore.Context;
using TechStore.Models;

namespace TechStore.Infrastructure
{
    public class CategoryRepository : Repository<Category, int>, ICategoryRepository
    {
        private readonly TechStoreContext _context;

        public CategoryRepository(TechStoreContext context) : base(context)
        {
            _context = context; 
        }

        public async Task<List<Category>> SearchByName(string name)
        {
            var data= await _context.Categories.Where(c=>c.Name==name).Select(c=>c).ToListAsync();
            return data;
        }


        

    }
}
