using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Dtos.OrderDtos
{
    public class GetAllOrderDto
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? ShippingAddress { get; set; }
        public string? Phone { get; set; }
        public string? OrderStatus { get; set; }
        public decimal? TotalPrice { get; set; }

    }
}
