using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Dtos.ProductDtos
{
    public class SmartwatchItemDtos
    {
        public string WarrantyInformation { get; set; }////ضمان 
        public int Id { get; set; }
        public string WaterResistant { get; set; }
        public string CompatibleWithDevices { get; set; }

        public int ColorId { get; set; }
        public int ProductId { get; set; }
    }
}
