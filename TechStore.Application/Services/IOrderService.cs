using TechStore.Dtos.OrderDtos;
using TechStore.Dtos.ViewResult;
using TechStore.Models;

namespace TechStore.Application.Services
{
    public interface IOrderService
    {
        Task<ResultView<OrderDto>> CreateOrderAsync(OrderDto orderDto);
        Task<ResultDataList<GetAllOrderDto>> GetAllOrdersAsync();
        Task<ResultView<GetAllOrderDto>> GetOrderByIdAsync(int orderId);
        Task<ResultView<OrderDto>> HardDeleteOrderAsync(int orderId);
        Task<ResultView<OrderDto>> SoftDeleteOrderAsync(int orderId);
        Task<ResultView<OrderItemDto>> UpdateOrderItemQuantityAsync(int orderId, int orderItemId, int newQuantity);
        Task<ResultView<GetAllOrderDto>> GetOrderWithItems(int orderId);
        Task<ResultDataList<GetOrderDetailsDto>> GetOrderDetails(int orderId);
        Task<ResultView<OrderDto>> updateStatus(int OrderId, OrderStatus NewOrderStatus);
    }
}