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
        private readonly IProductItemService _productItemService;

        public ProductController(IProductService productService,IProductItemService productItemService)
        {
            _productService = productService;
            _productItemService = productItemService;
        }


        [HttpGet]//create update delete getone getall
        public async Task<IActionResult> GetAll(int itemsPerPage = 1, int pageNumber = 10)
        {
            try
            {
                if(pageNumber < 1)
                {
                    return NoContent();
                }
                var products = await _productService.GetAllPagination(itemsPerPage, pageNumber);
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
        [Route("/Get/{id:int}")]
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
        public async Task<ActionResult> Create(CreateOrUpdateProductDtos product)
        {
            if (ModelState.IsValid)
            {
                await _productService.Create(product);
                return Ok("Created Successfully!");
            }
            return BadRequest(ModelState);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> HardDeleteProduct(int id)
        {
            if(id > 0)
            {
                var product = await _productService.GetOne(id);
                if (product.Entity is null)
                {
                    return BadRequest("Product NotFound !");
                }
                var DeletedProduct = await _productService.HardDelete(product.Entity);
                if (DeletedProduct.IsSuccess)
                {
                    return Ok(DeletedProduct.Message);
                }
                return BadRequest(DeletedProduct.Message);
            }
            return BadRequest("Enter Valid Id !");
        }


        [HttpDelete]
        public async Task<IActionResult> SoftDeleteProduct(CreateOrUpdateProductDtos productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var DeletedProduct = await _productService.SoftDelete(productDto);
            if (DeletedProduct.IsSuccess)
            {
                return Ok(DeletedProduct.Entity);
            }
            return BadRequest(DeletedProduct.Message);
        }


        [HttpPut]
        public async Task<ActionResult> Update(CreateOrUpdateProductDtos product)
        {
            if (ModelState.IsValid)
            {
                var p = await _productService.Update(product);
                return Ok("Updated Successfully!");
            }
            return BadRequest(ModelState);
        }

    }
}
