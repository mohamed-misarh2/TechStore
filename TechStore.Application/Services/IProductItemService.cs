using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Dtos.ProductDtos;
using TechStore.Dtos.ViewResult;

namespace TechStore.Application.Services
{
    public interface IProductItemService
    {
        //Task<ResultView<CreateOrUpdateProductDtos>> Create(CreateOrUpdateProductDtos productDto);
        Task<ResultView<T>> Create<T>(T productDto) where T : class;
        Task<ResultView<T>> Update<T>(T productDto) where T : class;
        Task<ResultView<CreateOrUpdateProductDtos>> GetAll(CreateOrUpdateProductDtos productDto);
        Task SoftDelete(int productId);
    }
}
