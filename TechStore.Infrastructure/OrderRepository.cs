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
    public class OrderRepository : Repository<Order, int>, IOrderRepository
    {
        private readonly TechStoreContext _contex;
        public OrderRepository(TechStoreContext context) : base(context)
        {
            _contex = context;
        }

        public IQueryable<Order> SortedByDateAscending(DateTime orderDate)
        {
            return _contex.Orders.Where(o=>o.OrderDate==orderDate).OrderBy(o=>o.OrderDate);
        }

        public IQueryable<Order> SortedByDateDescending(DateTime orderDate)
        {
            return _contex.Orders.Where(o => o.OrderDate == orderDate).OrderByDescending(o => o.OrderDate);
        }

        public IQueryable<Order> SearchByOrderStatus(string orderStatus)
        {
            return _contex.Orders.Where(o => o.OrderStatus == orderStatus).Select(o=>o);
        }

        public IQueryable<Order> SearchByOrderUser(string userId)
        {
            return _contex.Orders.Where(o => o.UserId == userId).Select(o => o);
        }
    }
}
