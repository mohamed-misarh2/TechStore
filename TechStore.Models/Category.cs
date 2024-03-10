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

        // Navigation property
       
        public ICollection<Product> Products { get; set; }

        public Category ()
        {
            Products = new List<Product> ();
        }
    }
}
