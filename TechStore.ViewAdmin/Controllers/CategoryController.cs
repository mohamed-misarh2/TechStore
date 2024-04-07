using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechStore.Application.Services;
using TechStore.Dtos.CategoryDtos;
using TechStore.Dtos.ProductDtos;

namespace TechStore.ViewAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService) 
        {
            _categoryService = categoryService;
        }


        [HttpPost]
        public async Task<IActionResult> Create( CategorySpecificationDto data)
        {
            var result = await _categoryService.CreateCategory(data.Category, data.SpecificationsDtos);
            if (result.IsSuccess)
            {
                return Ok(result.Entity);
            }
            return BadRequest(result.Message);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CategorySpecificationDto data)
        {
            var result = await _categoryService.UpdateCategory(data.Category, data.SpecificationsDtos);
            return Ok(result);
        }

        [HttpDelete("HardDelete")]
        public async Task<IActionResult> HardDelete(int id)
        {
            var data = await _categoryService.HardDeleteCategory(id);
            return Ok(data);
        }

        [HttpDelete("SoftDelete")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var data = await _categoryService.SoftDeleteCategory(id);
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _categoryService.GetAllCategory();
            return Ok(data);
        }

        [HttpGet]
        [Route("{Id:int}")]

        public async Task<IActionResult> GetByID(int Id)
        {
            var data = await _categoryService.GetCategoryById(Id);
            return Ok(data);
        }

        [HttpGet("SearchCategoriesByName")]
        //[Route("{Name}")]
        public async Task<IActionResult> GetByName(string Name)
        {
            var data = await _categoryService.GetCategoryByName(Name);
            return Ok(data);
        }


        [HttpDelete("DeleteSpec")]
        public async Task<IActionResult> DeleteSpec(int CategoryId, int SpecID)
        {
            var res = await _categoryService.DeleteSpecFromCategory(CategoryId, SpecID);
            return Ok(res);
        }

        [HttpPost("CreateSpec")]
        public async Task<IActionResult> CreateSpec(int CategoryId, SpecificationsDto specificationsDto)
        {
            var res = await _categoryService.AddSpecToCategory(CategoryId, specificationsDto);
            return Ok(res);
        }

        [HttpGet("GetSpecficationsByCategoryId")]
        public async Task<IActionResult> GetSpecficationsByCategoryId(int CategoryId)
        {
            var res = await _categoryService.GetSpecificationsByCategoryId(CategoryId);
            return Ok(res);
        }
    }
}
