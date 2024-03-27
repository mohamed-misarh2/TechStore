using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using TechStore.Application.Services;
using TechStore.Dtos.ProductDtos;
using TechStore.ViewUser.Models;

namespace TechStore.ViewUser.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Mobile()
        {
            var products = await _productService.GetAllPagination(5,1);
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string name, int itemsPerPage = 10, int pageNumber = 1)
        {
            try
            {
                var result = await _productService.SearchProduct(name, itemsPerPage, pageNumber);
                return View(result);
            }
            catch (ArgumentException ex)
            {

                return View("Error", new ErrorViewModel { Message = ex.Message });
            }

        }

        [HttpGet]
        public async Task<IActionResult> Filter(FillterProductsDtos criteria)
        {
            try
            {
                var result = await _productService.FilterProducts(criteria);
                return View("Filter", result.Entities); 
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = "An error occurred while processing your request." });
            }
        }

    }
}
