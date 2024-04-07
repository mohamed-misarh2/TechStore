using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TechStore.Application.Services;
using TechStore.ViewUser.Models;

namespace TechStore.ViewUser.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
             private IProductService _productService;
        public HomeController(ILogger<HomeController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        public async Task<IActionResult>  Index()
        {
            var productsResultTask = _productService.FilterNewlyAddedProducts(10);
            var productsResult = await productsResultTask;

            ViewBag.Products = productsResult.Entities.Take(5);
            ViewBag.Products2 = productsResult.Entities.Skip(5).Take(5);


            return View();
        }
        public IActionResult Cart()
        {
            return View("Cart");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
