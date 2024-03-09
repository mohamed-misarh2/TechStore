using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Dtos.OrderDtos
{
    public class OrderDtos
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }= DateTime.Now;  
        public string? ShippingAddress { get; set; }
        public string? ShippingMethod { get; set; }
        public decimal? TotalPrice { get; set; }
        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }
        public DateTime DeliveryDate { get; set; }
    }
}
