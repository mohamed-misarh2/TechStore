using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Models;

namespace TechStore.Application.Contract
{
    public interface IOrderRepository:IRepository<Order,int>
    {
        Task<Order> GetOrderWithItemsAsync(int orderId);
        Task<List<Order>> GetOrdersSortedByDateAscendingAsync();
        Task<List<Order>> GetOrdersSortedByDateDescendingAsync();
        Task<List<Order>> SearchOrdersAsync(string searchTerm);
        Task<List<Order>> GetOrdersByUserIdAsync(string userId);

    }
}
