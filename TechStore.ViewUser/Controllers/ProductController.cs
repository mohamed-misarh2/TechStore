using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using TechStore.Application.Services;
using TechStore.Dtos.ProductDtos;
using TechStore.Models;
using TechStore.ViewUser.Models;
using Microsoft.AspNetCore.Http;
using TechStore.ViewUser.ExtenstionMethods;

namespace TechStore.ViewUser.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductController(IProductService productService, IHttpContextAccessor httpContextAccessor)
        {
            _productService = productService;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Mobile()
        {
            var brands = await _productService.GetAllBrands();
            ViewBag.Brands = brands;
            var products = await _productService.FilterProductsByCategory(2, 5, 1);
            return View("ProductsByCategory", products);
        }
        public async Task<IActionResult> Laptop()
        {
            var products = await _productService.FilterProductsByCategory(2, 5, 1);
            return View("ProductsByCategory", products);
        } 
        public async Task<IActionResult> Screen()
        {
            var products = await _productService.FilterProductsByCategory(3, 5, 1);
            return View("ProductsByCategory", products);
        } 
        public async Task<IActionResult> SmartWatch()
        {
            var products = await _productService.FilterProductsByCategory(4, 5, 1);
            return View("ProductsByCategory",products);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string name)
        {
            try
            {
                var result = await _productService.SearchProduct(name);
                return View(result);
            }
            catch (ArgumentException ex)
            {

                return View("Error", new ErrorViewModel { Message = ex.Message });
            }

        }
    
        public async Task<IActionResult> Filter(FillterProductsDtos criteria, int itemsPerPage = 10, int pageNumber = 1)
        {
            try
            {
                var brands = await _productService.GetAllBrands();
                ViewBag.Brands = brands;

                if (ViewBag.Brands == null)
                {
                    ViewBag.Brands = new List<string>(); 
                }

                var result = await _productService.FilterProducts(criteria,10,1);
                return View("ProductsByCategory", result);
            }
            catch (Exception)
            {
                return View("Error", new ErrorViewModel { Message = "An error occurred while processing your request." });
            }
        }
        
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var result = await _productService.GetOne(id);
                return View("Details",result);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Message=ex.Message });
            }
        }

        


    }
}
