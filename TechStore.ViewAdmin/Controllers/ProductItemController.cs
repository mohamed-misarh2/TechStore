using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechStore.Application.Services;
using TechStore.Dtos.ProductDtos;

namespace TechStore.ViewAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductItemController : ControllerBase
    {
        private readonly IProductItemService _productItemService;

        public ProductItemController(IProductItemService productItemService)
        {
            _productItemService = productItemService;
        }

        //[HttpGet]
        //[Route("/Get/{id:int}")]
        //public async Task<IActionResult> GetOne(int id)
        //{
        //    if(id < 1)
        //    {
        //        return BadRequest("Enter Valid Id !");
        //    }

        //    var productItem = await _productItemService.GetOneProductItem(id);
        //    if (productItem.Entity is null)
        //    {
        //        return Ok("Not Found !");
        //    }
        //    return Ok(productItem.Message);
        //}


        //[HttpPost]
        //public async Task<IActionResult> Create(ProductItemDtos productItemDto)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        await _productItemService.CreateProductItem(productItemDto);
        //        return Ok("ProductItem Created Successfully !");
        //    }
        //    return BadRequest(ModelState);
        //}


        //[HttpDelete("{id}")]
        //public async Task<IActionResult> HardDelete(int id)
        //{
        //    if(id < 1)
        //    {
        //        return BadRequest("Product Valid Id !");
        //    }

        //    var ProductItemDtoo = await _productItemService.GetOneProductItem(id);
        //    if(ProductItemDtoo.Entity is null)
        //    {
        //        return BadRequest(ProductItemDtoo.Message);
        //    }

        //    var DeletedProductItem = await _productItemService.HardDeleteProductItem(ProductItemDtoo.Entity);
        //    return Ok(DeletedProductItem.Message);

        //}


        //[HttpDelete]
        //public async Task<IActionResult> SoftDelete(ProductItemDtos productItemDto)
        //{
        //    if(productItemDto.ProductId < 1)
        //    {
        //        return BadRequest("Product Valid Id !");
        //    }

        //    var ProductItemDtoo = await _productItemService.GetOneProductItem(productItemDto.ProductId);
        //    if (ProductItemDtoo.Entity is null)
        //    {
        //        return BadRequest(ProductItemDtoo.Message);
        //    }

        //    var DeletedProductItem = await _productItemService.SoftDeleteProductItem(ProductItemDtoo.Entity.ProductId);
        //    if (DeletedProductItem.IsSuccess)
        //    {
        //        return Ok(DeletedProductItem.Message);
        //    }
        //    return BadRequest(DeletedProductItem.Message);
        //}


        //[HttpPut]
        //public async Task<IActionResult> Update(ProductItemDtos productItemDto)
        //{
        //    if(productItemDto.ProductId < 1)
        //    {
        //        return BadRequest("Enter Valid Id !");
        //    }

        //    var ProductItemDtoo = await _productItemService.GetOneProductItem(productItemDto.ProductId);
        //    if (!ProductItemDtoo.IsSuccess)
        //    {
        //        return BadRequest(ProductItemDtoo.Message);
        //    }

        //    var UpdatedProductItem = await _productItemService.UpdateProductItem(ProductItemDtoo.Entity);
        //    if (UpdatedProductItem.IsSuccess)
        //    {
        //        return Ok(UpdatedProductItem.Message);
        //    }

        //    return BadRequest(UpdatedProductItem.Message);
        //}
    }
}
