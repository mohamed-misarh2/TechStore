using Microsoft.AspNetCore.Mvc;
using TechStore.Application.Services;

namespace TechStore.ViewUser.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService) {
            _productService = productService;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
