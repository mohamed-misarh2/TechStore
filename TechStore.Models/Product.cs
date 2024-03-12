using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Models
{
    public class Product :BaseEntity
    {
        public string Description { get; set; }
        public string Brand { get; set; }
        public string ModelName { get; set; }
        public DateTime DateAdded { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string UserId { get; set; }
        public TechUser User { get; set; }
        public ICollection<Image> Images { get; set; }
        public ICollection<ProductItem> ProductItem { get; set; }
        public ICollection<Review> Reviews { get; set;}
        public ICollection<OrderItem> OrderItems { get; set; }
        public Product()
        {
            OrderItems = new List<OrderItem>();
            Reviews = new List<Review>();
            ProductItem = new List<ProductItem>();
            Images= new List<Image>();  
        }
    }
}
