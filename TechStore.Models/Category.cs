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
        public int? ParentCategoryID { get; set; } 

        // Navigation property
        public Category ParentCategory { get; set; }
        public List<Category> ChildCategories { get; set; }
        public List<Product> Products { get; set; }
    }
}
