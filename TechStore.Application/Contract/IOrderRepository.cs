using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Models;

namespace TechStore.Application.Contract
{
    public interface IOrderRepository:IRepository<Order , int>
    {
        IQueryable<Order> SearchByOrderStatus(string orderStatus);      
        IQueryable<Order> SearchByOrderUser(string userId);
        IQueryable<Order> SortedByDateAscending(DateTime orderDate);
        IQueryable<Order> SortedByDateDescending(DateTime orderDate);
    }
}
