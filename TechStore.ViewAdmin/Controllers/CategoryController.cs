﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechStore.Application.Services;
using TechStore.Dtos.CategoryDtos;

namespace TechStore.ViewAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService) 
        {
            _categoryService = categoryService;
        }
        //[HttpPost]
        //public async Task<IActionResult> Create( CategoryDto category,List<SpecificationsDto> specificationsDtos)
        //{
        //    var data = await _categoryService.CreateCategory(category, specificationsDtos);
        //    return Ok(data);    
        //}

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategorySpecificationDto data)
        {
            var result = await _categoryService.CreateCategory(data.Category, data.SpecificationsDtos);
            if (result.IsSuccess)
            {
                return Ok(result.Entity);
            }
            return BadRequest(result.Message);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CategoryDto category)
        {
            var data = await _categoryService.UpdateCategory(category);
            return Ok(data);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(CategoryDto category)
        {
            var data = await _categoryService.SoftDeleteCategory(category);
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

        [HttpGet]
        [Route("{Name}")]
        public async Task<IActionResult> GetByName(string Name)
        {
            var data = await _categoryService.GetCategoryByName(Name);
            return Ok(data);
        }
    }
}
