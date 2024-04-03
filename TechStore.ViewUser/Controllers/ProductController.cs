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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductController(IProductService productService,IHttpContextAccessor httpContextAccessor)
        {
            _productService = productService;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Mobile(int pageNumber=1, int pageSize = 3)
        {
            var products = await _productService.FilterProductsByCategory(1, pageSize, pageNumber);
            ViewBag.PageNumber = pageNumber;
            ViewBag.ActionName = "Mobile";
            return View("ProductsByCategory", products);
        }
        public async Task<IActionResult> Laptop(int pageNumber = 1, int pageSize = 3)
        {
            var products = await _productService.FilterProductsByCategory(2, pageSize, pageNumber);
            ViewBag.PageNumber = pageNumber;
            ViewBag.ActionName = "Laptop";
            return View("ProductsByCategory", products);
        } 
        public async Task<IActionResult> Screen(int pageNumber = 1, int pageSize = 3)
        {
            var products = await _productService.FilterProductsByCategory(3, pageSize, pageNumber);
            ViewBag.PageNumber = pageNumber;
            ViewBag.ActionName = "Screen";
            return View("ProductsByCategory", products);
        } 
        public async Task<IActionResult> SmartWatch(int pageNumber = 1, int pageSize = 3)
        {
            var products = await _productService.FilterProductsByCategory(4, pageSize, pageNumber);
            ViewBag.PageNumber = pageNumber;
            ViewBag.ActionName = "SmartWatch";
            return View("ProductsByCategory",products);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string name, int pageSize = 6, int pageNumber = 1)
        {
            try
            {
                var result = await _productService.SearchProduct(name, pageSize, pageNumber);
                ViewBag.PageNumber = pageNumber;

                return View(result);
            }
            catch (ArgumentException ex)
            {

                return View("Error", new ErrorViewModel { Message = ex.Message });
            }

        }
        //public   IActionResult Filter()
        //{
        //    var filter = new FillterProductsDtos();
        //   // filter.Brand = await _productService.GetBrands();
        //    return View("Fillter", filter);
        //}
        //[HttpPost]
        public async Task<IActionResult> Filter(FillterProductsDtos criteria, int itemsPerPage = 10, int pageNumber = 1)
        {
            try
            {
                ViewBag.Brands = await _productService.GetBrands(1);

                // Ensure ViewBag.Brands is not null before passing it to the view
                if (ViewBag.Brands == null)
                {
                    ViewBag.Brands = new List<string>(); // Initialize an empty list to avoid null reference
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

