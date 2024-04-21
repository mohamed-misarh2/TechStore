﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Dtos.ProductDtos
{
    public class SpecificationsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; } = true;
    }
}
