using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Application.Contract;
using TechStore.Context;
using TechStore.Dtos.OrderDtos;
using TechStore.Models;

namespace TechStore.Infrastructure
{
    public class OrderRepository : Repository<Order, int>, IOrderRepository
    {
        public OrderRepository(TechStoreContext context) : base(context)
        {
        }

        public async Task<Order> GetOrderWithItemsAsync(int orderId)
        {
            var order = await _context.Orders
                .Where(order => order.Id == orderId)
                .Include(order => order.OrderItems)
                .Select(order => new Order
                {
                    Id = order.Id,
                    UserId = order.UserId,
                    OrderDate = order.OrderDate,
                    ShippingAddress = order.ShippingAddress,
                    ShippingMethod = order.ShippingMethod,
                    orderStatus = order.orderStatus,
                    PaymentStatus = order.PaymentStatus,
                    TotalPrice = order.TotalPrice,
                    OrderItems = order.OrderItems.Select(orderitem => new OrderItem
                    {
                        Id = orderitem.Id,
                        OrderId = orderitem.OrderId,
                        ProductId = orderitem.ProductId,
                        Quantity = orderitem.Quantity ?? 0,
                        UnitPrice = orderitem.UnitPrice ?? 0,
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return order;
        }

        public async Task<List<Order>> GetOrdersSortedByDateAscendingAsync()
        {
            return await _context.Orders.OrderBy(order => order.OrderDate).ToListAsync();
        }

        public async Task<List<Order>> GetOrdersSortedByDateDescendingAsync()
        {
            return await _context.Orders.OrderByDescending(order => order.OrderDate).ToListAsync();
        }

        public async Task<List<Order>> SearchOrdersAsync(string searchTerm)
        {
            searchTerm = searchTerm.ToLowerInvariant();

            return await _context.Orders
                .Where(order =>
                    order.UserId.ToLowerInvariant().Contains(searchTerm) ||
                    order.ShippingAddress.ToLowerInvariant().Contains(searchTerm) ||
                    order.ShippingMethod.ToLowerInvariant().Contains(searchTerm) ||
                    order.orderStatus.ToString().ToLowerInvariant().Contains(searchTerm) ||
                    order.PaymentStatus.ToLowerInvariant().Contains(searchTerm)
                )
                .ToListAsync();
        }
        public async Task<List<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _context.Orders
                .Where(order => order.UserId == userId)
                .ToListAsync();
        }
    }
}
