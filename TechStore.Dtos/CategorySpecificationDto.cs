using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Dtos.CategoryDtos;

namespace TechStore.Dtos
{
    public class CategorySpecificationDto
    {
        public CategoryDto Category { get; set; }
        public List<SpecificationsDto> SpecificationsDtos { get; set; }
    }
}
