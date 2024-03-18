using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Models
{
    public class CategorySpecifications:BaseEntity
    {
        public int CategoryId {  get; set; } 
        public Category Category { get; set; }

        public int SpecificationId { get; set; }
        public Specification Specification { get; set; }
    }
}
