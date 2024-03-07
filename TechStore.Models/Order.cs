using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Models
{
    public class Order : BaseEntity
    {
        public int TechUserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string? ShippingAddress { get; set; }
        public string? ShippingMethod { get; set; }
        public decimal? TotalPrice { get; set; }
        public string? OrderStatus { get; set; }
        public string?  PaymentStatus { get; set; }
        public DateTime DeliveryDate { get; set; }

        // Define relationships
        public TechUser User { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public List<Payment> Payments { get; set; }
        //mahmoud
    }
}
