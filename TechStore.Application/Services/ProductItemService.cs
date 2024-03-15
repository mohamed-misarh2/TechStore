using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Application.Contract;
using TechStore.Dtos.ProductDtos;
using TechStore.Dtos.ViewResult;
using TechStore.Models;

namespace TechStore.Application.Services
{
    public class ProductItemService : IProductItemService
    {
        private readonly IProductItemRepository _productItemRepository;
        private readonly IMapper _mapper;

        public ProductItemService(IProductItemRepository productItemRepository, IMapper mapper)
        {
            _productItemRepository = productItemRepository;
            _mapper = mapper;
        }

        public async Task<ResultView<T>> Create<T>(T productDto) where T : class
        {
            if (productDto is null)
            {
                throw new ArgumentNullException("Product DTO cannot be null");
            }

            var product = _mapper.Map<ProductItem>(productDto);
            var newProduct = await _productItemRepository.CreateAsync(product);
            await _productItemRepository.SaveChangesAsync();
            T NewProductDto;

            if (productDto is LabtopItemDtos)
            {
                NewProductDto = _mapper.Map<T>(newProduct);
            }
            else if(productDto is MobileAndTabletItemDtos)
            {
                NewProductDto = _mapper.Map<T>(newProduct);
            }
            else if(productDto is ScreenItemDtos)
            {
                NewProductDto = _mapper.Map<T>(newProduct);
            }
            else if(productDto is SmartwatchItemDtos)
            {
                NewProductDto = _mapper.Map<T>(newProduct);
            }
            else
            {
                throw new ArgumentException("Invalid DTO type");
            }

            return new ResultView<T>
            {
                Entity = NewProductDto,
                IsSuccess = true,
                Message = "Added successfully"
            };
        }

        public async Task SoftDelete(int productId)
        {
            if (productId <= 0)
            {
                throw new ArgumentException("Invalid productId");
            }

            var OldProductItem = await _productItemRepository.GetByIdAsync(productId);
            if (OldProductItem != null)
            {
                OldProductItem.IsDeleted = true;
                await _productItemRepository.SaveChangesAsync(); 
            }
            throw new ArgumentException("Product not found");
        }

        public async Task<ResultDataList<T>> GetAll<T>() where T : class //mobile
        {
            var productItems = await _productItemRepository.GetAllAsync();
            var productItemsDto = _mapper.Map<IQueryable<T>>(productItems);
            return new ResultDataList<T>
            {
                Entities = productItemsDto.ToList(),
                Count = productItemsDto.Count()
            };
        }

        public async Task<ResultView<T>> Update<T>(T productDto) where T : class
        {
            if (productDto is null)
            {
                throw new ArgumentNullException("Product DTO cannot be null");
            }
            var product = _mapper.Map<ProductItem>(productDto);
            var NewProduct = await _productItemRepository.UpdateAsync(product);
            await _productItemRepository.SaveChangesAsync();
            T NewProducDto;

            if (productDto is LabtopItemDtos)
            {
                NewProducDto = _mapper.Map<T>(NewProduct);
            }
            else if (productDto is MobileAndTabletItemDtos)
            {
                NewProducDto = _mapper.Map<T>(NewProduct);
            }
            else if (productDto is ScreenItemDtos)
            {
                NewProducDto = _mapper.Map<T>(NewProduct);
            }
            else if (productDto is SmartwatchItemDtos)
            {
                NewProducDto = _mapper.Map<T>(NewProduct);
            }
            else
            {
                throw new ArgumentException("Invalid DTO type");
            }

            return new ResultView<T>
            {
                Entity = NewProducDto,
                IsSuccess = true,
                Message = "Added successfully"
            };

        }

        public async Task<ResultView<T>> GetOne<T>(int id) where T : class
        {
            var ProductItem = await _productItemRepository.GetByIdAsync(id);
            if(ProductItem == null)
            {
                throw new ArgumentNullException("ProductItem Isn't Exist");
            }
            var ProductItemDto = _mapper.Map<T>(ProductItem);
            return new ResultView<T>
            {
                Entity = ProductItemDto,
                IsSuccess = true,
                Message = "ProductItem Retrived Successfully"
            };
        }
    }
}
