using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Models
{
    public class Category :BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ParentCategoryId { get; set; } 

        // Navigation property
        public Category ParentCategory { get; set; }
        public ICollection<Category> ChildCategories { get; set; }
        public ICollection<Product> Products { get; set; }

        public Category ()
        {
            ChildCategories = new List<Category> ();
            Products = new List<Product> ();
        }
    }
}
