using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Dtos.ProductDtos;
using TechStore.Dtos.ViewResult;
using System.Text.Json;

namespace TechStore.Application.Services
{
    public class CartService : ICartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _cartSessionKey = "CartItems";
        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        
        public Task<ResultView<CartItemDto>> AddToCart(CartItemDto cartItemDto)
        {
            throw new NotImplementedException();
        }

        public Task<ResultView<decimal>> CalcTotalPrice(List<CartItemDto> cartItemDto)
        {
            throw new NotImplementedException();
        }

        public Task<ResultDataList<CartItemDto>> GetAllCartItems()
        {
            throw new NotImplementedException();
        }

        public Task<ResultDataList<CartItemDto>> RemoveAllCartItems()
        {
            throw new NotImplementedException();
        }

        public Task<ResultView<CartItemDto>> RemoveOneFromCart(int ProductId)
        {
            throw new NotImplementedException();
        }
    }
}
