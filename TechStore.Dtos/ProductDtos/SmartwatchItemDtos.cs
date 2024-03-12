using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Dtos.ProductDtos
{
    public class SmartwatchItemDtos
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public string Type { get; set; }
        public string WarrantyInformation { get; set; }////ضمان 
        public string WaterResistant { get; set; }
        public string CompatibleWithDevices { get; set; }

        public int ColorId { get; set; }
        public int ProductId { get; set; }
    }
}
