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

        //
        public async Task<ResultView<OrderDto>> CreateOrderAsync(OrderDto orderDto)//user
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
                await _orderRepository.SaveChangesAsync();
                await _orderItemRepository.SaveChangesAsync();

                var createdOrderDto = _mapper.Map<OrderDto>(createdOrder);

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
        
        //
        public async Task<ResultView<OrderItemDto>> UpdateOrderItemQuantityAsync(int orderId, int orderItemId, int newQuantity)//user
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

                int quantityDifference;
                if (newQuantity > orderItem.Quantity)  
                {
                    quantityDifference =  newQuantity - (orderItem.Quantity ?? 0); 
                    product.Quantity -= quantityDifference;
                }
                else  
                {
                    quantityDifference = (orderItem.Quantity ?? 0) - newQuantity;
                    product.Quantity += quantityDifference;
                }

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
        
        //  ??
        public async Task<ResultView<GetAllOrderDto>> GetOrderByIdAsync(int orderId)//?
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
        
        //  want to edit with getorderdetails dto
        public async Task<ResultView<GetAllOrderDto>> GetOrderWithItems(int orderId)//admin
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
       
        //
        public async Task<ResultDataList<GetOrderDetailsDto>> GetOrderDetails(int orderId)//user
        { 
            var ExistingOrder = await _orderRepository.GetByIdAsync(orderId);

            if(ExistingOrder != null)
            {
                var OrderItems = await _orderItemRepository.GetOrders(orderId);

                var list = new List<GetOrderDetailsDto>();


                foreach (var OrderItem in OrderItems)
                {
                    var product = await _productRepository.GetByIdAsync(OrderItem.ProductId);
                    var images = await _productRepository.GetImagesByProductId(OrderItem.ProductId);
                    var obj = new GetOrderDetailsDto
                    {
                        Id = OrderItem.Id,
                        OrderId = OrderItem.OrderId,
                        ProductId = OrderItem.ProductId,
                        Description = OrderItem.Product.Description,
                        Price = OrderItem.Product.Price,
                        Quantity = OrderItem.Product.Quantity,
                        Image = OrderItem.Product.Images.Select(i => i.Name).FirstOrDefault()
                    };
                    list.Add(obj);
                }

                var listDto = _mapper.Map<List<GetOrderDetailsDto>>(list);

                return new ResultDataList<GetOrderDetailsDto>()
                {
                    Entities = listDto,
                    Count = listDto.Count
                };

            }
            else
            {
                return new ResultDataList<GetOrderDetailsDto>()
                {
                    Entities = null,
                    Count = 0
                };
            }
        }

        //
        public async Task<ResultDataList<GetAllOrderDto>> GetAllOrdersAsync()//admin
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
        
        //
        public async Task<ResultView<OrderDto>> SoftDeleteOrderAsync(int orderId)//user & admin
        {
            var result = new ResultView<OrderDto>();

            try
            {
                var order = await _orderRepository.GetByIdAsync(orderId);
                if (order == null)
                    throw new Exception($"Order with ID {orderId} not found.");

                order.IsDeleted = true;
                await _orderRepository.UpdateAsync(order);

                var OrderItems = await _orderItemRepository.GetOrders(orderId);//how binding in order ????
                foreach (var item in OrderItems)
                {
                    item.IsDeleted = true;
                    await _orderItemRepository.UpdateAsync(item);
                }
                
                await _orderRepository.SaveChangesAsync();
                await _orderItemRepository.SaveChangesAsync();
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


        public async Task<ResultView<OrderItemDto>> SoftDeleteOrderItemAsync(int orderItemId)
        {
            var result = new ResultView<OrderItemDto>();
            var ExistingOrderItem = await _orderItemRepository.GetByIdAsync(orderItemId);
            if(ExistingOrderItem != null)
            {
                ExistingOrderItem.IsDeleted = true;
                await _orderItemRepository.UpdateAsync(ExistingOrderItem);
                await _orderItemRepository.SaveChangesAsync();
                var orderItemDto = _mapper.Map<OrderItemDto>(ExistingOrderItem);
                result.Entity = orderItemDto;
                result.IsSuccess = true;
                result.Message = "OrderItem Deleted Successfully";

            }
            else
            {
                result.Entity = null;
                result.IsSuccess = false;
                result.Message = "Faild To Delete OrderItem";
            }
            return result;
        }
        //
        public async Task<ResultView<OrderDto>> HardDeleteOrderAsync(int orderId)//user & admin
        {
            var result = new ResultView<OrderDto>();

            try
            {
                var deletedOrder = await _orderRepository.GetByIdAsync(orderId);
                if (deletedOrder == null)
                    throw new Exception($"Order with ID {deletedOrder.Id} not found.");

                var deletedOrderItems = await _orderItemRepository.GetOrders(orderId);
                foreach (var item in deletedOrderItems)
                {
                    await _orderItemRepository.DeleteAsync(item);
                }

                await _orderItemRepository.SaveChangesAsync();  //delete orderitems then delete order
                await _orderRepository.DeleteAsync(deletedOrder);
                await _orderRepository.SaveChangesAsync();

                deletedOrder.OrderItems = deletedOrderItems;
                var deletedOrderDto = _mapper.Map<OrderDto>(deletedOrder);

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

        //
        public async Task<ResultDataList<GetAllOrderDto>> GetOrdersSortedByDateAscendingAsync()//user & admin
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

        //
        public async Task<ResultDataList<GetAllOrderDto>> GetOrdersSortedByDateDescendingAsync()//user & admin
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

        
        public async Task<ResultDataList<GetAllOrderDto>> SearchOrdersAsync(string searchTerm)//user & admin
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

        //
        public async Task<ResultDataList<GetAllOrderDto>> GetOrdersByUserIdAsync(string userId) //user
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

        //
        public async Task<ResultView<OrderDto>> updateStatus(int OrderId , OrderStatus NewOrderStatus)//admin
        {
            var ExistingOrder = await _orderRepository.GetByIdAsync(OrderId);
            var result = new ResultView<OrderDto>();
            if (ExistingOrder != null)
            {
                ExistingOrder.orderStatus = NewOrderStatus;
                await _orderRepository.UpdateAsync(ExistingOrder);
                await _orderRepository.SaveChangesAsync();
                var orderItems =await _orderItemRepository.GetOrders(OrderId);
                ExistingOrder.OrderItems = orderItems;
                var UpdatedOrderDto = _mapper.Map<OrderDto>(ExistingOrder);

                result.Entity = UpdatedOrderDto;
                result.IsSuccess = true;
                result.Message = "OrderStatus Updated Successfully";
            }
            else
            {
                result.Entity = null;
                result.IsSuccess = false;
                result.Message = "Failed to update order status";
            }
            return result;
        }
    
    }

}
