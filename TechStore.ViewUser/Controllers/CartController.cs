using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TechStore.Dtos.ProductDtos;
using TechStore.ViewUser.ExtenstionMethods;

namespace TechStore.ViewUser.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult AddToCart(CartItemDto cartItemDto)
        
        {
            var cart = HttpContext.Session.Get<List<CartItemDto>>("Cart") ?? new List<CartItemDto>();
            var existingItem = cart.FirstOrDefault(item => item.ProductId == cartItemDto.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += cartItemDto.Quantity;
            }
            else
            {
                cart.Add(cartItemDto);

            }

            HttpContext.Session.Set("Cart", cart);

            return RedirectToAction("Cart", "Cart");

        }

        public IActionResult RemoveFromCart(int productId)
        {
            var cart = HttpContext.Session.Get<List<CartItemDto>>("Cart") ?? new List<CartItemDto>();
            var itemToRemove = cart.FirstOrDefault(item => item.ProductId == productId);

            if (itemToRemove != null)
            {
                if (itemToRemove.Quantity > 1)
                {
                    itemToRemove.Quantity--;
                }
                else
                {
                    cart.Remove(itemToRemove);
                }
                HttpContext.Session.Set("Cart", cart);
            }

            return RedirectToAction("Cart", "Cart");
        }

        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            var cart = HttpContext.Session.Get<List<CartItemDto>>("Cart") ?? new List<CartItemDto>();
            var existingItem = cart.FirstOrDefault(item => item.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity = quantity;

                HttpContext.Session.Set("Cart", cart);
            }

            return RedirectToAction("Cart");
        }

        public IActionResult Cart()
        {
            var sessionCartItems =  HttpContext.Session.Get<List<CartItemDto>>("Cart");
            return View(sessionCartItems);
        }

    }
}
