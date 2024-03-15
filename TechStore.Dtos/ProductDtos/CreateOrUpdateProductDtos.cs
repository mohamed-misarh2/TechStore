using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Dtos.ProductDtos
{
    public class CreateOrUpdateProductDtos
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public string ModelName { get; set; }
        public DateTime DateAdded { get; set; }
        public int CategoryId { get; set; }
        public string UserId { get; set; }
        public List<IFormFile>? Images { get; set; }
    }
}
