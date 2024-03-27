using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Application.Contract;
using TechStore.Dtos.OrderDtos;
using TechStore.Dtos.ViewResult;
using TechStore.Models;

namespace TechStore.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, IProductRepository productRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ResultView<OrderDto>> CreateOrderAsync(OrderDto orderDto)
        {
            var result = new ResultView<OrderDto>();

            try
            {
                var order = _mapper.Map<Order>(orderDto);
                order.OrderDate = DateTime.Now;

                var orderItems = _mapper.Map<List<OrderItem>>(orderDto.OrderItems);

                foreach (var orderItem in orderItems)
                {
                    var product = await _productRepository.GetByIdAsync(orderItem.ProductId);
                    if (product == null)
                        throw new Exception($"Product with ID {orderItem.ProductId} not found.");

                    // Set the unit price of the order item to the actual price of the product
                    orderItem.UnitPrice = product.Price;

                    // Adjust product quantity
                    product.Quantity -= orderItem.Quantity ?? 0;
                    await _productRepository.UpdateAsync(product);
                    await _productRepository.SaveChangesAsync();
                }

                // Calculate the total price based on order item quantities and unit prices
                order.TotalPrice = orderItems.Sum(item => (item.Quantity ?? 0) * item.UnitPrice);

                order.OrderItems = orderItems;

                var createdOrder = await _orderRepository.CreateAsync(order);
                var createdOrderDto = _mapper.Map<OrderDto>(createdOrder);
                await _orderRepository.SaveChangesAsync();
                await _orderItemRepository.SaveChangesAsync();

                result.IsSuccess = true;
                result.Message = "Order created successfully.";
                result.Entity = createdOrderDto;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = $"Failed to create order: {ex.Message}";
            }

            return result;
        }


        public async Task<ResultView<OrderItemDto>> UpdateOrderItemQuantityAsync(int orderId, int orderItemId, int newQuantity)
        {
            var result = new ResultView<OrderItemDto>();

            try
            {
                var orderItem = await _orderItemRepository.GetByIdAsync(orderItemId);
                if (orderItem == null)
                    throw new Exception("Order item not found.");

                var product = await _productRepository.GetByIdAsync(orderItem.ProductId);
                if (product == null)
                    throw new Exception("Product not found.");

                var quantityDifference = newQuantity - (orderItem.Quantity ?? 0);
                product.Quantity -= quantityDifference;

                orderItem.Quantity = newQuantity;
                await _orderItemRepository.UpdateAsync(orderItem);
                await _orderItemRepository.SaveChangesAsync();


                await _productRepository.UpdateAsync(product);
                await _productRepository.SaveChangesAsync();


                var updatedOrderItemDto = _mapper.Map<OrderItemDto>(orderItem);

                result.IsSuccess = true;
                result.Message = "Order item quantity updated successfully.";
                result.Entity = updatedOrderItemDto;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = $"Failed to update order item quantity: {ex.Message}";
            }

            return result;
        }
        public async Task<ResultView<GetAllOrderDto>> GetOrderByIdAsync(int orderId)
        {
            var result = new ResultView<GetAllOrderDto>();

            try
            {
                var order = await _orderRepository.GetByIdAsync(orderId);
                if (order == null)
                    throw new Exception($"Order with ID {orderId} not found.");

                var orderDto = _mapper.Map<GetAllOrderDto>(order);

                result.IsSuccess = true;
                result.Message = "Order retrieved successfully.";
                result.Entity = orderDto;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = $"Failed to retrieve order: {ex.Message}";
            }

            return result;
        }

        public async Task<ResultView<GetAllOrderDto>> GetOrderWithItems(int orderId)
        {
            var result = new ResultView<GetAllOrderDto>();
            try
            {
                var order = await _orderRepository.GetOrderWithItemsAsync(orderId);
                var orderDto = _mapper.Map<GetAllOrderDto>(order);
                result.Entity = orderDto;
                result.Message = "Order returned Successfully";
                result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                result.Entity = null;
                result.Message = $"Error {ex.Message}";
                result.IsSuccess = false;
            }
            return result;
        }
        public async Task<ResultDataList<GetAllOrderDto>> GetAllOrdersAsync()
        {
            var result = new ResultDataList<GetAllOrderDto>();

            try
            {
                var orders = await _orderRepository.GetAllAsync();
                //var allOrders = orders.Include(order=>order.OrderItems).ToListAsync();
                var orderDtos = _mapper.Map<List<GetAllOrderDto>>(orders);

                result.Entities = orderDtos;
                result.Count = orderDtos.Count;
            }
            catch (Exception ex)
            {
                result.Entities = null;
                result.Count = 0;
            }

            return result;
        }
        public async Task<ResultView<OrderDto>> SoftDeleteOrderAsync(int orderId)
        {
            var result = new ResultView<OrderDto>();

            try
            {
                var order = await _orderRepository.GetByIdAsync(orderId);
                if (order == null)
                    throw new Exception($"Order with ID {orderId} not found.");

                order.IsDeleted = true;
                await _orderRepository.UpdateAsync(order);

                var orderDto = _mapper.Map<OrderDto>(order);

                result.IsSuccess = true;
                result.Message = "Order deleted successfully.";
                result.Entity = orderDto;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = $"Failed to delete order: {ex.Message}";
            }

            return result;
        }

        public async Task<ResultView<OrderDto>> HardDeleteOrderAsync(OrderDto orderDto)
        {
            var result = new ResultView<OrderDto>();

            try
            {
                var order = _mapper.Map<Order>(orderDto);

                var deletedOrder = await _orderRepository.GetByIdAsync(order.Id);
                if (deletedOrder == null)
                    throw new Exception($"Order with ID {deletedOrder.Id} not found.");

                await _orderRepository.DeleteAsync(deletedOrder);

                var deletedOrderDto = _mapper.Map<OrderDto>(order);

                result.IsSuccess = true;
                result.Message = "Order  deleted successfully.";
                result.Entity = deletedOrderDto;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = $"Failed to  delete order: {ex.Message}";
            }

            return result;
        }

        public async Task<ResultDataList<GetAllOrderDto>> GetOrdersSortedByDateAscendingAsync()
        {
            var result = new ResultDataList<GetAllOrderDto>();

            try
            {
                var orders = await _orderRepository.GetOrdersSortedByDateAscendingAsync();
                var orderDtos = _mapper.Map<List<GetAllOrderDto>>(orders);

                result.Entities = orderDtos;
                result.Count = orderDtos.Count;
            }
            catch (Exception ex)
            {
                result.Entities = null;
                result.Count = 0;
            }

            return result;
        }

        public async Task<ResultDataList<GetAllOrderDto>> GetOrdersSortedByDateDescendingAsync()
        {
            var result = new ResultDataList<GetAllOrderDto>();

            try
            {
                var orders = await _orderRepository.GetOrdersSortedByDateDescendingAsync();
                var orderDtos = _mapper.Map<List<GetAllOrderDto>>(orders);

                result.Entities = orderDtos;
                result.Count = orderDtos.Count;
            }
            catch (Exception ex)
            {
                result.Entities = null;
                result.Count = 0;
            }

            return result;
        }

        public async Task<ResultDataList<GetAllOrderDto>> SearchOrdersAsync(string searchTerm)
        {
            var result = new ResultDataList<GetAllOrderDto>();

            try
            {
                var orders = await _orderRepository.SearchOrdersAsync(searchTerm);
                var orderDtos = _mapper.Map<List<GetAllOrderDto>>(orders);

                result.Entities = orderDtos;
                result.Count = orderDtos.Count;
            }
            catch (Exception ex)
            {
                result.Entities = null;
                result.Count = 0;
            }

            return result;
        }
        public async Task<ResultDataList<GetAllOrderDto>> GetOrdersByUserIdAsync(string userId)
        {
            var result = new ResultDataList<GetAllOrderDto>();

            try
            {
                var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
                var orderDtos = _mapper.Map<List<GetAllOrderDto>>(orders);

                result.Entities = orderDtos;
                result.Count = orderDtos.Count;
               
            }
            catch (Exception ex)
            {
                result.Entities = null;
                result.Count = 0;
                
            }

            return result;
        }
    }

}
