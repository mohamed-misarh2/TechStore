using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Models
{
    public class Image:BaseEntity
    {
        public string Name { get; set; } 
        [ForeignKey("ProductItem")]
        public int ItemId { get; set; }
        public ProductItem ProductItem { get; set; }

    }
}
