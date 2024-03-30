﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Dtos
{
    public class ProductItemDto
    {
        public int SpecificationID { get; set; }
        public string Brand { get; set; }
        public string ModelName { get; set; }
        public string Color { get; set; }
        public string StorageCapacity { get; set; }
        public string RAM { get; set; }
        public string RearCameraResolution { get; set; }
        public string FrontCamera { get; set; }
        public string NumberOfSIMCards { get; set; }
        public string OperatingSystem { get; set; }
        public string ProcessorBrand { get; set; }
        public string NumberOfProcessorCores { get; set; }
        public string BatteryCapacity { get; set; }
        public string Network { get; set; }
        public string PortType { get; set; }
        public string Resolution { get; set; }
        public string ScreenSize { get; set; }
    }
}