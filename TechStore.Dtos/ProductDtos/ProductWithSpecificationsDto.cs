﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Dtos.ProductDtos
{
    public class ProductWithSpecificationsDto
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public string ModelName { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountValue { get; set; }
        public decimal? DiscountedPrice { get; set; }
        public string? Warranty { get; set; }
        public int? Quantity { get; set; }
        public DateTime DateAdded { get; set; }
        public string? UserId { get; set; }
        public List<IFormFile> Images { get; set; }

        public List<ProductCategorySpecificationsDto> ProductCategorySpecifications { get; set; }
        public ProductWithSpecificationsDto()
        {
            DateAdded = DateTime.Now;
            Images = new List<IFormFile>();
        }
    }
}
