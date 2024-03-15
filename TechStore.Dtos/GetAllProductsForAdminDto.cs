using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Models;

namespace TechStore.Dtos
{
    public class GetAllProductsForAdminDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public ICollection<string> Images { get; set; }
        public int CategoryId { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsDeleted { get; set; }
        public string FullName { get; set; }
    }

}
