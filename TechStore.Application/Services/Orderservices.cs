using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Application.Contract;
using TechStore.Dtos.OrderDtos;
using TechStore.Dtos.ViewResult;
using TechStore.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TechStore.Application.Services
{
    public class Orderservices : IOrderSevices
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public Orderservices(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<ResultView<OrderDtos>> CreateOrder(OrderDtos order)
        {
            var oldorder=(await _orderRepository.GetAllAsync()).Where(o=>o.Id==order.Id).FirstOrDefault();
            if (oldorder!=null)
            {
                return new ResultView<OrderDtos> { Entity = null, IsSuccess = false, Message = "Order Already Exist" };
            }
            else
            {
                var orderM = _mapper.Map<Order>(order);
                var newOrder=_orderRepository.CreateAsync(orderM);
                await _orderRepository.SaveChangesAsync();
                var data = _mapper.Map<OrderDtos>(newOrder);
                return new ResultView<OrderDtos> { Entity= data , IsSuccess = true, Message = "Created Successflly" };

            }
        }
        public async Task<ResultView<OrderDtos>> UpdatOrder(OrderDtos order)
        {
            var oldorder = (await _orderRepository.GetAllAsync()).Where(o => o.Id == order.Id).FirstOrDefault();
            if (oldorder == null)
            {
                return new ResultView<OrderDtos> { Entity = null, IsSuccess = false, Message = "Order Not Found" };
            }
            else
            {
                var orderM = _mapper.Map<Order>(order);
                var updateOrder = _orderRepository.UpdateAsync(orderM);
                await _orderRepository.SaveChangesAsync();
                var data = _mapper.Map<OrderDtos>(updateOrder);
                return new ResultView<OrderDtos> { Entity = data, IsSuccess = true, Message = "Updated Successflly" };

            }
        }
        public async Task<ResultView<OrderDtos>> HardDeleteOrder(OrderDtos order)
        {
            var oldorder = (await _orderRepository.GetAllAsync()).Where(o => o.Id == order.Id).FirstOrDefault();
           
            try
            {
                if (oldorder == null)
                {
                    return new ResultView<OrderDtos> { Entity = null, IsSuccess = false, Message = "Order Not Found" };
                }
                else
                {
                    var orderM = _mapper.Map<Order>(order);
                    var deleteOrder = _orderRepository.DeleteAsync(orderM);
                    await _orderRepository.SaveChangesAsync();
                    var data = _mapper.Map<OrderDtos>(deleteOrder);
                    return new ResultView<OrderDtos> { Entity = data, IsSuccess = true, Message = "Deleted Successflly" };

                }
            }
            catch (Exception ex)
            {

                return new ResultView<OrderDtos> { Entity = null, IsSuccess = false, Message =ex.Message };

            }

        }

        public async Task<ResultView<OrderDtos>> SoftDeleteOrder(OrderDtos order)
        {
            var oldorder = (await _orderRepository.GetAllAsync()).Where(o => o.Id == order.Id).FirstOrDefault();

            try
            {
                if (oldorder == null)
                {
                    return new ResultView<OrderDtos> { Entity = null, IsSuccess = false, Message = "Order Not Found" };
                }
                else
                {
                    var orderM = _mapper.Map<Order>(order);
                    var deleteOrder = (await _orderRepository.GetAllAsync()).FirstOrDefault(o => o.Id == order.Id);
                    deleteOrder.IsDeleted=true;
                    await _orderRepository.SaveChangesAsync();
                    var data = _mapper.Map<OrderDtos>(deleteOrder);
                    return new ResultView<OrderDtos> { Entity = data, IsSuccess = true, Message = "Deleted Successflly" };

                }
            }
            catch (Exception ex)
            {

                return new ResultView<OrderDtos> { Entity = null, IsSuccess = false, Message = ex.Message };

            }
        }

        public async Task<ResultDataList<OrderDtos>> GetAllOrderPagination(int items, int pageNumber)
        {
            var allOrders=await _orderRepository.GetAllAsync();
            var orders=allOrders.Skip(items*pageNumber).Take(items).Select(o=>new OrderDtos
                                                                            {
                                                                                Id= o.Id,
                                                                                UserId=o.UserId,
                                                                                OrderDate=o.OrderDate,
                                                                                ShippingAddress=o.ShippingAddress,
                                                                                ShippingMethod=o.ShippingMethod,
                                                                                TotalPrice=o.TotalPrice,
                                                                                OrderStatus=o.OrderStatus,
                                                                                PaymentStatus=o.PaymentStatus,
                                                                                DeliveryDate=o.DeliveryDate,

                                                                            }).ToList();
            ResultDataList<OrderDtos> resultDataList = new ResultDataList<OrderDtos>();
            resultDataList.Entities= orders;
            resultDataList.Count = allOrders.Count();
            return resultDataList;
        }

        public async Task<OrderDtos> GetOrderBYID(int id)
        {
            var order=await _orderRepository.GetByIdAsync(id);
            var data = _mapper.Map<OrderDtos>(order);
            return data;
        }

        public OrderDtos FilterByUser(string userId)
        {
            var orderM = _orderRepository.SearchByOrderUser(userId);
            var orders = _mapper.Map<OrderDtos>(orderM);
            return orders;
        }

        public OrderDtos FilterByOrderStatus(string orderStatus)
        {
            var orderM=_orderRepository.SearchByOrderStatus(orderStatus);
            var orders = _mapper.Map<OrderDtos>(orderM);
            return orders;
        }

        public OrderDtos OrderedByDateAscending(DateTime orderDate)
        {
            var orderM = _orderRepository.SortedByDateAscending(orderDate);
            var orders = _mapper.Map<OrderDtos>(orderM);
            return orders; throw new NotImplementedException();
        }

        public OrderDtos OrderedByDateDescending(DateTime orderDate)
        {
            var orderM = _orderRepository.SortedByDateDescending(orderDate);
            var orders = _mapper.Map<OrderDtos>(orderM);
            return orders; throw new NotImplementedException();
        }
    }
}
