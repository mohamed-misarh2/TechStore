using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Models
{
    public class OrderItem
    {
        
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubtotalPrice => Quantity * UnitPrice; // Calculated property ??

        // Navigation properties
        public int OrderId { get; set; } // Foreign Key
        public Order Order { get; set; }
        public int ProductId { get; set; } // Foreign Key
        public Product Product { get; set; }
    }
}
