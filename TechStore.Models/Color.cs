using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Models
{
    public class Color:BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public ICollection<ProductItem> ProductItems { get; set; }
        public Color()
        {
            ProductItems = new List<ProductItem>();
        }
    }
}
