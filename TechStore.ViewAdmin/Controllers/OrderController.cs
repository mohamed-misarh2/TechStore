using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechStore.Application.Services;
using TechStore.Dtos.OrderDtos;
using TechStore.Dtos.ViewResult;

namespace TechStore.ViewAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> create([FromBody] OrderDto orderDto)
        {
            var product = await _orderService.CreateOrderAsync(orderDto);
            return Ok(product);
        }

        [HttpPut("update")]
        public async Task<IActionResult> update( int orderId, int orderItemId, int newQuantity)
        {
            var product = await _orderService.UpdateOrderItemQuantityAsync(orderId,orderItemId,newQuantity);
            return Ok(product);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrdersAsync(int ItemsPerPage, int PageNumber)
        {
            var result = await _orderService.GetAllPaginationOrders(ItemsPerPage, PageNumber);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var result = await _orderService.GetOrderDetails(id);
            return Ok(result);
        }
        
        [HttpDelete("SoftDeleteOrder")]
        public async Task<IActionResult> SoftDeleteOrder(int id)
        {
            var result  = await _orderService.SoftDeleteOrderAsync(id);
            return Ok(result);
        }

        [HttpDelete("SoftDeleteOrderItem")]
        public async Task<IActionResult> SoftDeleteOrderItem(int id)
        {
            var result = await _orderService.SoftDeleteOrderAsync(id);
            return Ok(result);
        }

        [HttpDelete("HardDeleteOrder")]
        public async Task<IActionResult> HardDeleteOrder(int id)
        {
            var result = await _orderService.HardDeleteOrderAsync(id);
            return Ok(result);
        }

        [HttpPut("updateStatus")]
        public async Task<IActionResult> updateStatus(int OrderId, Models.OrderStatus NewOrderStatus)
        {
            var state = await _orderService.updateStatus(OrderId, NewOrderStatus);
            return Ok(state);
        }

        [HttpGet("GetSortedAs")]
        public async Task<IActionResult> GetSortedAs()
        {
            var result = await _orderService.GetOrdersSortedByDateAscendingAsync();
            return Ok(result);
        }

        [HttpGet("GetSortedes")]
        public async Task<IActionResult> GetSortedes()
        {
            var result = await _orderService.GetOrdersSortedByDateDescendingAsync();
            return Ok(result);
        }

        [HttpGet("GetorderByUserId")]
        public async Task<IActionResult> GetorderByUserId(string userId)
        {
            var result = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(result);
        }

        [HttpGet("searchOrder")]
        public async Task<IActionResult> searchOrder(string term)
        {
            var result = await _orderService.SearchOrdersAsync(term);
            return Ok(result);
        }
    }
}
