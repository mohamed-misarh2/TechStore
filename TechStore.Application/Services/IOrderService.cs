using TechStore.Dtos.OrderDtos;
using TechStore.Dtos.ViewResult;

namespace TechStore.Application.Services
{
    public interface IOrderService
    {
        Task<ResultView<OrderDto>> CreateOrderAsync(OrderDto orderDto);
        Task<ResultDataList<GetAllOrderDto>> GetAllOrdersAsync();
        Task<ResultView<GetAllOrderDto>> GetOrderByIdAsync(int orderId);
        Task<ResultView<OrderDto>> HardDeleteOrderAsync(OrderDto orderDto);
        Task<ResultView<OrderDto>> SoftDeleteOrderAsync(int orderId);
        Task<ResultView<OrderItemDto>> UpdateOrderItemQuantityAsync(int orderId, int orderItemId, int newQuantity);
        Task<ResultView<GetAllOrderDto>> GetOrderWithItems(int orderId);

    }
}