﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Dtos.ProductDtos
{
    public class ProductItemDtos
    {
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }/// 
        public decimal? DiscountPrice { get; set; }
        public string? ScreenSize { get; set; }
        public string? ScreenType { get; set; }
        public string? Resoluation { get; set; }
        public string? Type { get; set; }
        public string? WarrantyInformation { get; set; }////ضمان 
        public string? OS { get; set; }//mobile , lab
        public string? ProcessorBrand { get; set; } //mobile ,lab
        public string? Ram { get; set; }//mobile , lab
        public string? CameraResolution { get; set; }//mobile
        public string? FrontCamera { get; set; }//mobile
        public string? NumberSIMCards { get; set; }//mobile
        public string? StorageCapacity { get; set; }//mobile 
        public string? BatteryCapacity { get; set; }//mobile
        public string? RAMType { get; set; }//lab
        public string? GraphicsCard { get; set; }//lab
        public string? GraphicsCardType { get; set; }//lab
        public string? USBPorts { get; set; }//lab
        public string? PortType { get; set; }//lab
        public string? NumberProcessorCores { get; set; }//lab
        public string? ProcessorSpeed { get; set; }//lab
        public string? ScreenDesign { get; set; }//Screen
        public string? Clarity { get; set; }//Screen الوضوح
        public string? WaterResistant { get; set; }//SmartWatch مقاوم للماء
        public string? CompatibleWithDevices { get; set; }//SmartWatch متوافق مع اجهزه 
        public int ColorId { get; set; }
        public int ProductId { get; set; }
    }
}
