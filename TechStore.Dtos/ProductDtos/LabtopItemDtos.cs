using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Dtos.ProductDtos
{
    public class LabtopItemDtos
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public string ScreenSize { get; set; }
        public string ScreenType { get; set; }
        public string Resoluation { get; set; }
        public string Type { get; set; }
        public string WarrantyInformation { get; set; }////ضمان 
        public string OS { get; set; }
        public string ProcessorBrand { get; set; } 
        public string Ram { get; set; }
        public string RAMType { get; set; }
        public string GraphicsCard { get; set; }
        public string GraphicsCardType { get; set; }
        public string USBPorts { get; set; }
        public string PortType { get; set; }
        public string NumberProcessorCores { get; set; }
        public string ProcessorSpeed { get; set; }
        public int ColorId { get; set; }
        public int ProductId { get; set; }
    }
}
