using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Models
{
    public class ProductSpecifications:BaseEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int SpecificationId { get; set; }
        public Specification Specification { get; set; }

        public string Value { get; set; }
    }
}
