using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Models
{
    public class Order : BaseEntity
    {
        public string? UserId { get; set; }
        public DateTime? OrderDate { get; set; }= DateTime.Now;
        public string? ShippingAddress { get; set; }
        public string? ShippingMethod { get; set; }
        public decimal? TotalPrice { get; set; }
        public string? OrderStatus { get; set; }
        public string?  PaymentStatus { get; set; }
        public DateTime? DeliveryDate { get; set; }      

        // Define relationships
        public TechUser? User { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }
        public ICollection<Payment>? Payments { get; set; }

        public Order()
        {
            OrderItems = new List<OrderItem>();
            Payments = new List<Payment>();
        }
    }
}
