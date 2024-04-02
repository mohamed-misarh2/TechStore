using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Dtos.ProductDtos;
using TechStore.Dtos.ViewResult;

namespace TechStore.Application.Services
{
    public interface ICartService
    {
        Task<ResultView<CartItemDto>> AddToCart(CartItemDto cartItemDto);
        Task<ResultView<CartItemDto>> RemoveOneFromCart(int ProductId);
        Task<ResultDataList<CartItemDto>> GetAllCartItems();
        Task<ResultDataList<CartItemDto>> RemoveAllCartItems();
        Task<ResultView<decimal>> CalcTotalPrice(List<CartItemDto> cartItemDto);

    }
}
