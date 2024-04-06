using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks.Sources;
using TechStore.Application.Services;
using TechStore.Dtos.ProductDtos;
using TechStore.Models;

namespace TechStore.ViewAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }


        [HttpGet("GetAll")]//create update delete getone getall
        public async Task<IActionResult> GetAll(int pageIteme=10 , int pageNumber=1)
        {
            try
            { 
                var products = await _productService.GetAllPagination(pageIteme , pageNumber);

                if(products.Count == 0)
                {
                    return NoContent();
                }
                else
                {
                    return Ok(products);    
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet]
        [Route("/GetOneById/{id:int}")]
        public async Task<IActionResult> GetOneById(int id)
        {
            if (id < 0)
            {
                return BadRequest("Enter Valid Id !");
            }

            var product = await _productService.GetOne(id);
            if (product.Entity is null)
            {
                return Ok("Not Found");
            }
            else
            {
                return Ok(product);
            }
        }


        [HttpPost]
        public async Task<ActionResult> Create(ProductCategorySpecificationsListDto product)
        {
            if (ModelState.IsValid)
            {
                var res =  await _productService.Create(product.CreateOrUpdateProductDtos,product.ProductCategorySpecifications);
                return Ok(res);
            }
            return BadRequest(ModelState);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> HardDeleteProduct(int id)
        {
            if (id > 0)
            {
                var DeletedProduct = await _productService.HardDelete(id);
                if (DeletedProduct.IsSuccess)
                {
                    return Ok(DeletedProduct.Message);
                }
                return BadRequest(DeletedProduct.Message);
            }
            return BadRequest("Enter Valid Id !");
        }


        [HttpDelete]
        public async Task<IActionResult> SoftDeleteProduct(int ProductId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var DeletedProduct = await _productService.SoftDelete(ProductId);
            if (DeletedProduct.IsSuccess)
            {
                return Ok(DeletedProduct.Entity);
            }
            return BadRequest(DeletedProduct.Message);
        }


        [HttpPut]
        public async Task<ActionResult> Update([FromBody] ProductCategorySpecificationsListDto product)
        {
            if (ModelState.IsValid)
            {
                var Product = await _productService.Update(product.CreateOrUpdateProductDtos,product.ProductCategorySpecifications);
                return Ok("Updated Successfully!");
            }
            return BadRequest(ModelState);
        }

        //user

        [HttpGet("GetProductsByCategory/{categoryId:int}")]
        public async Task<IActionResult> GetProductsByCategory(int categoryId, int ItemsPerPage, int PageNumber)
        {
            if (ModelState.IsValid)
            {
                var Products = await _productService.FilterProductsByCategory(categoryId, ItemsPerPage , PageNumber);
                return Ok(Products);
            }
            return BadRequest(ModelState);
        }


        [HttpPut("Filterr")]
        public async Task<IActionResult> Filterr(FillterProductsDtos fillterProductsDtos, int ItemsPerPage, int PageNumber)
        {
            if (ModelState.IsValid)
            {
                var products = await _productService.FilterProducts(fillterProductsDtos, ItemsPerPage, PageNumber);
                return Ok(products);
            }
            return BadRequest(ModelState);
            
        }

        //user


        [HttpGet("SortProductsByDescending")]
        public async Task<IActionResult> SortProductsByDescending(int ItemsPerPage, int PageNumber)
        {
            if (ModelState.IsValid)
            {
                var products = await _productService.SortProductsByDesending(ItemsPerPage, PageNumber);
                return Ok(products);
            }
            return BadRequest(ModelState);

        }


        [HttpGet("SortProductsByAscending")]
        public async Task<IActionResult> SortProductsByAscending(int ItemsPerPage, int PageNumber)
        {
            if (ModelState.IsValid)
            {
                var products = await _productService.SortProductsByAscending(ItemsPerPage, PageNumber);
                return Ok(products);
            }
            return BadRequest(ModelState);

        }


        //search

        [HttpGet("SearchByName")]
        public async Task<IActionResult> SearchByName(string Name, int ItemsPerPage, int PageNumber)
        {
            if (ModelState.IsValid)
            {
                var products = await _productService.SearchProduct(Name);
                return Ok(products);
            }
            return BadRequest(ModelState);
        }

      

    }
}
