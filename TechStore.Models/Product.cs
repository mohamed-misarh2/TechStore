using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public int StockQuantity { get; set; }
        public List<string> Images { get; set; }
        //public string color {  get; set; }
        public decimal Rating { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public DateTime DateAdded { get; set; }
        public int categoryId { get; set; }
        public Category category { get; set; }
        public ICollection<OrderItem> items { get; set; }
        public ICollection<ProductItem> ProductItems { get; set; }
        public Product ()
        {
            items = new List<OrderItem> ();
            Reviews = new List<Review> ();
            ProductItems = new List<ProductItem> ();
        }
    }
}
