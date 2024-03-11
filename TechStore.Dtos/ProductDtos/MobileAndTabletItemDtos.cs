using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Dtos.ProductDtos
{
    public class MobileAndTabletItemDtos
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
        public string CameraResolution { get; set; }
        public string FrontCamera { get; set; }
        public string NumberSIMCards { get; set; }
        public string StorageCapacity { get; set; }
        public string BatteryCapacity { get; set; }
        public int ColorId { get; set; }
        public int ProductId { get; set; }
    }
}
