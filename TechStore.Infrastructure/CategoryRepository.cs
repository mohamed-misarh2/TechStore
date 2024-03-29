﻿using Microsoft.EntityFrameworkCore;
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
        public CategoryRepository(TechStoreContext context) : base(context)
        {
        }

        public async Task<Category> SearchByName(string name)
        {
            return await _entities.Where(c=>c.Name==name).FirstOrDefaultAsync();
        }


        

    }
}
