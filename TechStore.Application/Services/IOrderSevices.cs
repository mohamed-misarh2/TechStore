using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Dtos.OrderDtos;
using TechStore.Dtos.ViewResult;

namespace TechStore.Application.Services
{
    public interface IOrderSevices
    {
        Task<ResultView<OrderDtos>> CreateOrder(OrderDtos order);
        Task<ResultView<OrderDtos>> UpdatOrder(OrderDtos order);
        Task<ResultView<OrderDtos>> HardDeleteOrder(OrderDtos order);
        Task<ResultView<OrderDtos>> SoftDeleteOrder(OrderDtos order);
        Task<ResultDataList<OrderDtos>> GetAllOrderPagination(int items, int pageNumber);
        Task<OrderDtos> GetOrderBYID(int id);
        OrderDtos FilterByUser(string userId);
        OrderDtos FilterByOrderStatus(string orderStatus);
        OrderDtos OrderedByDateAscending(DateTime orderDate);
        OrderDtos OrderedByDateDescending(DateTime orderDate);
    }
    }
}
