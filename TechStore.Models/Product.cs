using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Models
{
    public class Product :BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public int StockQuantity { get; set; }
        public string Specifications { get; set; } // Serialized JSON string
        public string WarrantyInformation { get; set; }
        public List<string> Images { get; set; }
        public double AverageRating { get; set; }
        public List<Review> Reviews { get; set; }
        public DateTime DateAdded { get; set; }
        public int categoryId { get; set; }
        public Category category { get; set; }
        List<OrderItem> items { get; set; }
    }
}
