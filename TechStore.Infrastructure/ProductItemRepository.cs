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
    public class ProductItemRepository:Repository<ProductItem,int>,IProductItemRepository
    {
        public ProductItemRepository(TechStoreContext techStoreContext) : base(techStoreContext) { }



    }
}
