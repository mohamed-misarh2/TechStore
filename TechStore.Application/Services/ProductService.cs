using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TechStore.Application.Contract;
using TechStore.Dtos;
using TechStore.Dtos.ViewResult;
using TechStore.Models;

namespace TechStore.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository,IMapper mapper) {
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task<ResultView<ProductDto>> Create(ProductDto productDto)
        {
            var OldProduct = (await _productRepository.GetAllAsync())
                             .Where(p=>p.Id == productDto.Id).FirstOrDefault();
            if(OldProduct != null)
            {
                return new ResultView<ProductDto>
                {
                    Entity = null,
                    IsSuccess = false,
                    Message = "Product Already Exists !"
                };
            }

            var product = _mapper.Map<Product>(productDto);
            var NewProduct = await _productRepository.CreateAsync(product);
            await _productRepository.SaveChangesAsync();
            var NewProductDto = _mapper.Map<ProductDto>(product);
            return new ResultView<ProductDto>
            {
                Entity = NewProductDto,
                IsSuccess = true,
                Message = "Product Added Successfully !"
            };
        }

        public async Task<ResultView<ProductDto>> GetOne(int id)
        {
            var ProductModel =await _productRepository.GetByIdAsync(id);
            var productDto = _mapper.Map<ProductDto>(ProductModel);
            if (productDto != null)
            {
                return new ResultView<ProductDto>
                {
                    Entity = productDto,
                    IsSuccess = true,
                    Message = "Product Retrived Successfully !"
                };
            }

            return new ResultView<ProductDto>
            {
                Entity = null,
                IsSuccess = false,
                Message = "Product doesn't Exist !"
            };

        }

        public async Task<ResultView<ProductDto>> Update(ProductDto productDto)
        {
            var OldProductDto = (await _productRepository.GetAllAsync())
                                .Where(p => p.Id == productDto.Id).FirstOrDefault();
            if(OldProductDto != null)
            {
                var product = _mapper.Map<Product>(OldProductDto);
                var NewUpdatedProduct = await _productRepository.UpdateAsync(product);
                await _productRepository.SaveChangesAsync();
                var NewUpdatedProductDto = _mapper.Map<ProductDto>(NewUpdatedProduct);
                return new ResultView<ProductDto>
                {
                    Entity = NewUpdatedProductDto,
                    IsSuccess = true,
                    Message = "Product Updated Successfully !"
                };
            }

            return new ResultView<ProductDto>
            {
                Entity = null,
                IsSuccess = false,
                Message = "Product doesn't Exist & failed To Update !"
            };
        }

        public async Task<ResultView<ProductDto>> SoftDelete(ProductDto productDto)
        {
            //var OldProduct = (await _productRepository.GetAllAsync())
            //                 .Where(p=>p.Id == productDto.Id).FirstOrDefault();
            var OldProduct = (await _productRepository.GetByIdAsync(productDto.Id));
            if (OldProduct != null)
            {
                OldProduct.IsDeleted = true;
                await _productRepository.SaveChangesAsync();
                var DeletedProductDto = _mapper.Map<ProductDto>(OldProduct);
                return new ResultView<ProductDto>
                {
                    Entity = DeletedProductDto,
                    IsSuccess = true,
                    Message = "Product Deleted Successfully !"

                };
            }

            return new ResultView<ProductDto>
            {
                Entity = null,
                IsSuccess = false,
                Message = "failed tp delete this product !"

            };

        }

        public async Task<ResultDataList<GetAllProductsForAdminDto>> GetAllPaginationForAdmin(int ItemsPerPage, int PageNumber)
        {
            if(PageNumber <= 0)
            {
                throw new ArgumentException("Page number must be greater than zero");
            }

            var products = (await _productRepository.GetAllAsync())
                            .Where(p=>p.IsDeleted != false)
                            .Skip(ItemsPerPage * (PageNumber - 1)).Take(ItemsPerPage)
                            .Select(p => new GetAllProductsForAdminDto
                            {
                                Id = p.Id,
                                Name = p.Name,
                                Description = p.Description,
                                Brand = p.Brand,
                                Price = p.Price,
                                StockQuantity = p.StockQuantity,
                                Images = p.Images,
                                CategoryId = p.categoryId,
                                DateAdded = p.DateAdded,
                                IsDeleted = p.IsDeleted
                            }).ToList();

            var resultDataList = new ResultDataList<GetAllProductsForAdminDto>()
            {
                Entities = products,
                Count = products.Count()
            };
            return resultDataList;
        }

        public async Task<ResultDataList<ProductDto>> SearchProduct(string Name)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new ArgumentException("Name cannot be empty or whitespace.");
            }
            var products = await _productRepository.SearchProduct(Name);
            var ProductsDto = _mapper.Map<IQueryable<ProductDto>>(products);//tolist
            var resultDataList = new ResultDataList<ProductDto>()
            {
                Entities = ProductsDto.ToList(),
                Count = ProductsDto.Count()
            };
            return resultDataList;
        }

        public async Task<ResultDataList<ProductDto>> SearchByBrand(string Brand)
        {
            if (string.IsNullOrWhiteSpace(Brand))
            {
                throw new ArgumentException("Name cannot be empty or whitespace.");
            }
            var products = await _productRepository.SearchByBrand(Brand);
            var ProductsDto = _mapper.Map<IQueryable<ProductDto>>(products);
            var resultDataList = new ResultDataList<ProductDto>()
            {
                Entities = ProductsDto.ToList(),
                Count = ProductsDto.Count()
            };
            return resultDataList;
        }

        public async Task<ResultDataList<ProductDto>> GetProductsByCategory(int categoryId)
        {
            if(categoryId <= 0)
            {
                throw new ArgumentException("Invalid CategoryId");
            }
            var products = (await _productRepository.GetProductsByCategory(categoryId));
            var ProductsDto = _mapper.Map<IQueryable<ProductDto>>(products);
            var resultDataList = new ResultDataList<ProductDto>()
            {
                Entities = ProductsDto.ToList(),
                Count = ProductsDto.Count()
            };
            return resultDataList;
        }

        public async Task<ResultDataList<ProductDto>> GetRelatedProducts(int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            var products = await _productRepository.GetRelatedProducts(product);
            var ProductsDto = _mapper.Map<IQueryable<ProductDto>>(products);
            var resultDataList = new ResultDataList<ProductDto>()
            {
                Entities = ProductsDto.ToList(),
                Count = ProductsDto.Count()
            };
            return resultDataList;
        }

        public async Task<ResultDataList<ProductDto>> GetProductsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            if(minPrice < 0 || maxPrice < 0)
            {
                throw new ArgumentException("Prices cannot be negative !");
            }

            if(minPrice > maxPrice)
            {
                throw new ArgumentException("Minimum Price Cannot be Greater than Maximum Price");
            }

            var products = (await _productRepository.GetProductsByPriceRange(minPrice, maxPrice));
            var productsDtos = _mapper.Map<IQueryable<ProductDto>>(products);
            var resultDataList = new ResultDataList<ProductDto>()
            {
                Entities = productsDtos.ToList(),
                Count = productsDtos.Count()
            };
            return resultDataList;
        }

        public async Task<ResultDataList<ProductDto>> GetNewlyAddedProductsAsync(int count)
        {
            if (count <= 0)
            {
                throw new ArgumentException("The count must be greater than zero.", nameof(count));
            }
            var products = await _productRepository.GetNewlyAddedProducts(count);
            var ProductsDto = _mapper.Map<IQueryable<ProductDto>>(products);
            var resultDataLists = new ResultDataList<ProductDto>()
            {
                Entities = ProductsDto.ToList(),
                Count = ProductsDto.Count()
            };
            return resultDataLists;
        }

        public async Task<ResultDataList<ProductDto>> GetDiscountedProducts()
        {
            var products = await _productRepository.GetDiscountedProducts();
            if(products is null)//!products.Any() =>empty ?
            {
                throw new ArgumentException("No discounted products found");
            }
            var productsDto = _mapper.Map<IQueryable<ProductDto>>(products);
            var resultDataList = new ResultDataList<ProductDto>()
            {
                Entities = productsDto.ToList(),
                Count = productsDto.Count()
            };
            return resultDataList;
        }

        //public async Task<ResultDataList<GetAllProductsForUserDto>> GetAllPaginationForUser(int ItemsPerPage, int PageNumber)
        //{
        //    if (PageNumber <= 0)
        //    {
        //        throw new ArgumentException("Page number must be greater than zero");
        //    }

        //    var products = (await _productRepository.GetAllAsync())
        //                    .Where(p => p.IsDeleted != false)
        //                    .Skip(ItemsPerPage * (PageNumber - 1)).Take(ItemsPerPage)
        //                    .Select(p => new GetAllProductsForUserDto
        //                    {
        //                        Id = p.Id,
        //                        Name = p.Name,
        //                        Description = p.Description,
        //                        Brand = p.Brand,
        //                        Price = p.Price,
        //                        Images = p.Images,
        //                        CategoryId = p.categoryId,
        //                        DateAdded = p.DateAdded,
        //                        IsDeleted = p.IsDeleted
        //                    }).ToList();

        //    var resultDataList = new ResultDataList<GetAllProductsForUserDto>()
        //    {
        //        Entities = products,
        //        Count = products.Count()
        //    };
        //    return resultDataList;
        //}

    }
}
