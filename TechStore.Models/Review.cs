using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Models
{
    public class Review :BaseEntity
    {
        public int TechUserId { get; set; }
        public int ProductId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }

        // Define relationships
        public TechUser User { get; set; }
        public Product Product { get; set; }

    }
}
