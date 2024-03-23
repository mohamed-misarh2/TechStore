using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Application.Contract;
using TechStore.Dtos;
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
    }
}


        //public async Task<ResultView<T>> Create<T>(T productItemDto)
        //{
        //    if (productItemDto is null)
        //    {
        //        throw new ArgumentNullException("Product DTO cannot be null");
        //    }

        //    var product = _mapper.Map<Specification>(productItemDto);
        //    var newProduct = await _productItemRepository.CreateAsync(product);
        //    await _productItemRepository.SaveChangesAsync();
        //    T NewProductDto;

        //    if (productItemDto is LabtopItemDtos)
        //    {
        //        NewProductDto = _mapper.Map<T>(newProduct);
        //    }
        //    else if (productItemDto is MobileAndTabletItemDtos)
        //    {
        //        NewProductDto = _mapper.Map<T>(newProduct);
        //    }
        //    else if (productItemDto is ScreenItemDtos)
        //    {
        //        NewProductDto = _mapper.Map<T>(newProduct);
        //    }
        //    else if (productItemDto is SmartwatchItemDtos)
        //    {
        //        NewProductDto = _mapper.Map<T>(newProduct);
        //    }
        //    else
        //    {
        //        throw new ArgumentException("Invalid DTO type");
        //    }

        //    return new ResultView<T>
        //    {
        //        Entity = NewProductDto,
        //        IsSuccess = true,
        //        Message = "Added successfully"
        //    };
        //}

        //public async Task SoftDelete(int productId)
        //{
        //    if (productId <= 0)
        //    {
        //        throw new ArgumentException("Invalid productId");
        //    }

        //    var OldProductItem = await _productItemRepository.GetByIdAsync(productId);
        //    if (OldProductItem != null)
        //    {
        //        OldProductItem.IsDeleted = true;
        //        await _productItemRepository.SaveChangesAsync();
        //    }
        //    throw new ArgumentException("Product not found");
        //}

        //public async Task<ResultDataList<T>> GetAll<T>() where T : class //mobile
        //{
        //    var productItems = await _productItemRepository.GetAllAsync();
        //    var productItemsDto = _mapper.Map<IQueryable<T>>(productItems);
        //    return new ResultDataList<T>
        //    {
        //        Entities = productItemsDto.ToList(),
        //        Count = productItemsDto.Count()
        //    };
        //}

        //public async Task<ResultView<T>> Update<T>(T productDto) where T : class
        //{
        //    if (productDto is null)
        //    {
        //        throw new ArgumentNullException("Product DTO cannot be null");
        //    }
        //    var product = _mapper.Map<Specification>(productDto);
        //    var NewProduct = await _productItemRepository.UpdateAsync(product);
        //    await _productItemRepository.SaveChangesAsync();
        //    T NewProducDto;

        //    if (productDto is LabtopItemDtos)
        //    {
        //        NewProducDto = _mapper.Map<T>(NewProduct);
        //    }
        //    else if (productDto is MobileAndTabletItemDtos)
        //    {
        //        NewProducDto = _mapper.Map<T>(NewProduct);
        //    }
        //    else if (productDto is ScreenItemDtos)
        //    {
        //        NewProducDto = _mapper.Map<T>(NewProduct);
        //    }
        //    else if (productDto is SmartwatchItemDtos)
        //    {
        //        NewProducDto = _mapper.Map<T>(NewProduct);
        //    }
        //    else
        //    {
        //        throw new ArgumentException("Invalid DTO type");
        //    }

        //    return new ResultView<T>
        //    {
        //        Entity = NewProducDto,
        //        IsSuccess = true,
        //        Message = "Added successfully"
        //    };

        //}

        //public async Task<ResultView<T>> GetOne<T>(int id) where T : class
        //{
        //    var ProductItem = await _productItemRepository.GetByIdAsync(id);
        //    if (ProductItem == null)
        //    {
        //        throw new ArgumentNullException("ProductItem Isn't Exist");
        //    }
        //    var ProductItemDto = _mapper.Map<T>(ProductItem);
        //    return new ResultView<T>
        //    {
        //        Entity = ProductItemDto,
        //        IsSuccess = true,
        //        Message = "ProductItem Retrived Successfully"
        //    };
        //}
        //}


        //public async Task<ResultView<ProductItemDtos>> CreateProductItem(ProductItemDtos productItemDto)
        //{
        //    try
        //    {
        //        if (productItemDto is null)
        //        {
        //            throw new ArgumentNullException("ProductItem DTO cannot be null");
        //        }

        //        var ProductExisting = await _productItemRepository.GetByIdAsync(productItemDto.ProductId);//?? handle by unique
        //        if (ProductExisting != null)
        //        {
        //            throw new ArgumentNullException("ProductItem already exists");
        //        }

        //        var product = _mapper.Map<ProductItem>(productItemDto);
        //        var newProduct = await _productItemRepository.CreateAsync(product);
        //        await _productItemRepository.SaveChangesAsync();
        //        var newProductDto = _mapper.Map<ProductItemDtos>(newProduct);
        //        return new ResultView<ProductItemDtos>
        //        {
        //            Entity = newProductDto,
        //            IsSuccess = true,
        //            Message = "ProductItem Added successfully"
        //        };

        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResultView<ProductItemDtos>
        //        {
        //            Entity = null,
        //            IsSuccess = false,
        //            Message = ex.Message
        //        };
        //    }
        //}

        //public async Task<ResultView<ProductItemDtos>> SoftDeleteProductItem(int ProductItemId)
        //{
        //    try
        //    {
        //        if (ProductItemId <= 0)
        //        {
        //            throw new ArgumentException("Invalid ProductItemId");
        //        }

        //        var OldProductItem = await _productItemRepository.GetByIdAsync(ProductItemId);
        //        if (OldProductItem is null)
        //        {
        //            throw new ArgumentException("ProductItem not found");
        //        }

        //        OldProductItem.IsDeleted = true;
        //        await _productItemRepository.SaveChangesAsync();
        //        var NewProductDto = _mapper.Map<ProductItemDtos>(OldProductItem);
        //        return new ResultView<ProductItemDtos>
        //        {
        //            Entity = NewProductDto,
        //            IsSuccess = true,
        //            Message = "ProductItem Deleted Successfully"
        //        };

        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResultView<ProductItemDtos>
        //        {
        //            Entity = null,
        //            IsSuccess = false,
        //            Message = ex.Message
        //        };
        //    }

        //}

        //public async Task<ResultDataList<T>> GetAll<T>() where T : class //mobile ***** neeed????
        //{
        //    var productItems = await _productItemRepository.GetAllAsync();
        //    var productItemsDto = _mapper.Map<IQueryable<T>>(productItems);
        //    return new ResultDataList<T>
        //    {
        //        Entities = productItemsDto.ToList(),
        //        Count = productItemsDto.Count()
        //    };
        //}

    //    public async Task<ResultView<ProductItemDtos>> HardDeleteProductItem(ProductItemDtos productItemDto)
    //    {
    //        try
    //        {
    //            if (productItemDto is null)
    //            {
    //                throw new ArgumentNullException("ProductItem DTO cannot be null");
    //            }

    //            var productItem = await _productItemRepository.GetByIdAsync(productItemDto.ProductId);
    //            if (productItem is null)
    //            {
    //                throw new ArgumentNullException("ProductItem Not Found");
    //            }

    //            var DeletedproductItem = await _productItemRepository.DeleteAsync(productItem);
    //            var DeletedproductItemDto = _mapper.Map<ProductItemDtos>(DeletedproductItem);
    //            return new ResultView<ProductItemDtos>
    //            {
    //                Entity = DeletedproductItemDto,
    //                IsSuccess = true,
    //                Message = "ProductItem Deleted Successfully"

    //            };
    //        }
    //        catch (Exception ex)
    //        {
    //            return new ResultView<ProductItemDtos>
    //            {
    //                Entity = null,
    //                IsSuccess = false,
    //                Message = ex.Message
    //            };
    //        }

    //    }

    //    public async Task<ResultView<ProductItemDtos>> UpdateProductItem(ProductItemDtos productDto)
    //    {

    //        try
    //        {
    //            if (productDto is null)
    //            {
    //                throw new ArgumentNullException("Product DTO cannot be null");
    //            }

    //            var Oldproduct = await _productItemRepository.GetByIdAsync(productDto.ProductId);
    //            if (Oldproduct is null)
    //            {
    //                throw new ArgumentNullException("ProductItem Not Found");//InvalidOperationException ???
    //            }

    //            var product = _mapper.Map<ProductItem>(productDto);
    //            var NewProduct = await _productItemRepository.UpdateAsync(product);
    //            await _productItemRepository.SaveChangesAsync();
    //            var NewProductDto = _mapper.Map<ProductItemDtos>(NewProduct);
    //            return new ResultView<ProductItemDtos>
    //            {
    //                Entity = NewProductDto,
    //                IsSuccess = true,
    //                Message = "ProductItem Updated Successfully"
    //            };
    //        }
    //        catch (Exception ex)
    //        {
    //            return new ResultView<ProductItemDtos>
    //            {
    //                Entity = null,
    //                IsSuccess = true,
    //                Message = ex.Message
    //            };
    //        }
    //    }

    //    public async Task<ResultView<ProductItemDtos>> GetOneProductItem(int id)
    //    {
    //        try
    //        {
    //            var ProductItem = await _productItemRepository.GetByIdAsync(id);
    //            if (ProductItem == null)
    //            {
    //                throw new ArgumentNullException("ProductItem Isn't Exist");
    //            }
    //            var ProductItemDto = _mapper.Map<ProductItemDtos>(ProductItem);
    //            return new ResultView<ProductItemDtos>
    //            {
    //                Entity = ProductItemDto,
    //                IsSuccess = true,
    //                Message = "ProductItem Retrived Successfully"
    //            };
    //        }
    //        catch (Exception ex)
    //        {
    //            return new ResultView<ProductItemDtos>
    //            {
    //                Entity = null,
    //                IsSuccess = false,
    //                Message = ex.Message
    //            };

    //        }

    //    }

    //}

