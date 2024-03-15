using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TechStore.Application.Contract;
using TechStore.Dtos;
using TechStore.Dtos.ProductDtos;
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
        public async Task<ResultView<CreateOrUpdateProductDtos>> Create(CreateOrUpdateProductDtos productDto)
        {
            var OldProduct = (await _productRepository.GetAllAsync())
                             .Where(p=>p.Id == productDto.Id).FirstOrDefault();
            if(OldProduct != null)
            {
                return new ResultView<CreateOrUpdateProductDtos>
                {
                    Entity = null,
                    IsSuccess = false,
                    Message = "Product Already Exists !"
                };
            }

            var product = _mapper.Map<Product>(productDto);
            var NewProduct = await _productRepository.CreateAsync(product);
            await _productRepository.SaveChangesAsync();
            var NewProductDto = _mapper.Map<CreateOrUpdateProductDtos>(product);
            return new ResultView<CreateOrUpdateProductDtos>
            {
                Entity = NewProductDto,
                IsSuccess = true,
                Message = "Product Added Successfully !"
            };
        }

        public async Task<ResultView<CreateOrUpdateProductDtos>> GetOne(int id)
        {
            var ProductModel =await _productRepository.GetByIdAsync(id);
            var productDto = _mapper.Map<CreateOrUpdateProductDtos>(ProductModel);
            if (productDto != null)
            {
                return new ResultView<CreateOrUpdateProductDtos>
                {
                    Entity = productDto,
                    IsSuccess = true,
                    Message = "Product Retrived Successfully !"
                };
            }

            return new ResultView<CreateOrUpdateProductDtos>
            {
                Entity = null,
                IsSuccess = false,
                Message = "Product doesn't Exist !"
            };

        }

        public async Task<ResultView<CreateOrUpdateProductDtos>> Update(CreateOrUpdateProductDtos productDto)
        {
            var OldProductDto = (await _productRepository.GetAllAsync())
                                .Where(p => p.Id == productDto.Id).FirstOrDefault();
            if(OldProductDto != null)
            {
                var product = _mapper.Map<Product>(OldProductDto);
                var NewUpdatedProduct = await _productRepository.UpdateAsync(product);
                await _productRepository.SaveChangesAsync();
                var NewUpdatedProductDto = _mapper.Map<CreateOrUpdateProductDtos>(NewUpdatedProduct);
                return new ResultView<CreateOrUpdateProductDtos>
                {
                    Entity = NewUpdatedProductDto,
                    IsSuccess = true,
                    Message = "Product Updated Successfully !"
                };
            }

            return new ResultView<CreateOrUpdateProductDtos>
            {
                Entity = null,
                IsSuccess = false,
                Message = "Product doesn't Exist & failed To Update !"
            };
        }

        public async Task<ResultView<CreateOrUpdateProductDtos>> SoftDelete(CreateOrUpdateProductDtos productDto)
        {
            //var OldProduct = (await _productRepository.GetAllAsync())
            //                 .Where(p=>p.Id == productDto.Id).FirstOrDefault();
            var OldProduct = (await _productRepository.GetByIdAsync(productDto.Id));
            if (OldProduct != null)
            {
                OldProduct.IsDeleted = true;
                await _productRepository.SaveChangesAsync();
                var DeletedProductDto = _mapper.Map<CreateOrUpdateProductDtos>(OldProduct);
                return new ResultView<CreateOrUpdateProductDtos>
                {
                    Entity = DeletedProductDto,
                    IsSuccess = true,
                    Message = "Product Deleted Successfully !"

                };
            }

            return new ResultView<CreateOrUpdateProductDtos>
            {
                Entity = null,
                IsSuccess = false,
                Message = "failed tp delete this product !"

            };

        }
        
        public async Task<ResultView<CreateOrUpdateProductDtos>> HardDelete(CreateOrUpdateProductDtos productDto)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(productDto.Id);//??? whyyyyyyyyyy ???
                var DeletedProductModel = await _productRepository.DeleteAsync(product);
                if (DeletedProductModel == null)
                {
                    throw new ArgumentException("Product not found");
                }
                await _productRepository.SaveChangesAsync();
                var DeletedProductDto = _mapper.Map<CreateOrUpdateProductDtos>(DeletedProductModel);
                return new ResultView<CreateOrUpdateProductDtos>
                {
                    Entity = DeletedProductDto,
                    IsSuccess = true,
                    Message = "Product Deleted Sucessfully"
                };
            }
            catch(Exception ex)
            {
                return new ResultView<CreateOrUpdateProductDtos>
                {
                    Entity = null,
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResultDataList<GetAllProductsForAdminDto>> GetAllPaginationForAdmin(int ItemsPerPage, int PageNumber)
        {
            if(PageNumber <= 0)
            {
                throw new ArgumentException("Page number must be greater than zero");
            }

            var products = (await _productRepository.GetAllAsync())
                            //.Include(p => p.User)
                            .Where(p=>p.IsDeleted == false)
                            .Skip(ItemsPerPage * (PageNumber - 1)).Take(ItemsPerPage)
                            .Select(p => new GetAllProductsForAdminDto
                            {
                                Id = p.Id,
                                Name = p.ModelName,
                                Description = p.Description,
                                Brand = p.Brand,
                                CategoryId = p.CategoryId,
                                DateAdded = p.DateAdded,
                                FullName = $"{p.User.FirstName} {p.User.LastName}",
                                IsDeleted = p.IsDeleted
                            }).ToList();

            var resultDataList = new ResultDataList<GetAllProductsForAdminDto>()
            {
                Entities = products,
                Count = products.Count()
            };
            return resultDataList;
        }

        public async Task<ResultDataList<CreateOrUpdateProductDtos>> SearchProduct(string Name, int ItemsPerPage, int PageNumber)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new ArgumentException("Name cannot be empty or whitespace.");
            }

            if (PageNumber <= 0)
            {
                throw new ArgumentException("Page number must be greater than zero");
            }

            var products = (await _productRepository.SearchProduct(Name))
                           .Where(p => p.IsDeleted != false)
                           .Skip(ItemsPerPage * (PageNumber - 1))
                           .Take(ItemsPerPage)
                           .ToList();

            var ProductsDto = _mapper.Map<List<CreateOrUpdateProductDtos>>(products);//tolist
            var resultDataList = new ResultDataList<CreateOrUpdateProductDtos>()
            {
                Entities = ProductsDto,
                Count = ProductsDto.Count()
            };
            return resultDataList;
        }

        public async Task<ResultDataList<CreateOrUpdateProductDtos>> SearchByBrand(string Brand, int ItemsPerPage, int PageNumber)
        {
            if (string.IsNullOrWhiteSpace(Brand))
            {
                throw new ArgumentException("Name cannot be empty or whitespace.");
            }
            if (PageNumber <= 0)
            {
                throw new ArgumentException("Page number must be greater than zero");
            }

            var products = (await _productRepository.SearchByBrand(Brand))
                           .Where(p => p.IsDeleted != false)
                           .Skip(ItemsPerPage * (PageNumber - 1))
                           .Take(ItemsPerPage)
                           .ToList();

            var ProductsDto = _mapper.Map<List<CreateOrUpdateProductDtos>>(products);
            var resultDataList = new ResultDataList<CreateOrUpdateProductDtos>()
            {
                Entities = ProductsDto,
                Count = ProductsDto.Count()
            };
            return resultDataList;
        }

        public async Task<ResultDataList<CreateOrUpdateProductDtos>> FilterProductsByCategory(int categoryId, int ItemsPerPage, int PageNumber)
        {
            if(categoryId <= 0)
            {
                throw new ArgumentException("Invalid CategoryId");
            }

            if (PageNumber <= 0)
            {
                throw new ArgumentException("Page number must be greater than zero");
            }

            var products = (await _productRepository.GetProductsByCategory(categoryId))
                           .Where(p=>p.IsDeleted != false)
                           .Skip(ItemsPerPage * (PageNumber - 1))
                           .Take(ItemsPerPage)
                           .ToList();

            var ProductsDto = _mapper.Map<List<CreateOrUpdateProductDtos>>(products);
            var resultDataList = new ResultDataList<CreateOrUpdateProductDtos>()
            {
                Entities = ProductsDto,
                Count = ProductsDto.Count()
            };
            return resultDataList;
        }

        public async Task<ResultDataList<CreateOrUpdateProductDtos>> FiltertRelatedProducts(int productId, int ItemsPerPage, int PageNumber)
        {
            if (PageNumber <= 0)
            {
                throw new ArgumentException("Page number must be greater than zero");
            }

            var product = await _productRepository.GetByIdAsync(productId);
            var products = (await _productRepository.GetRelatedProducts(product))
                           .Where(p => p.IsDeleted != false)
                           .Skip(ItemsPerPage * (PageNumber - 1))
                           .Take(ItemsPerPage)
                           .ToList();

            var ProductsDto = _mapper.Map<List<CreateOrUpdateProductDtos>>(products);
            var resultDataList = new ResultDataList<CreateOrUpdateProductDtos>()
            {
                Entities = ProductsDto,
                Count = ProductsDto.Count()
            };
            return resultDataList;
        }

        public async Task<ResultDataList<CreateOrUpdateProductDtos>> FilterProductsByPriceRange(decimal minPrice, decimal maxPrice, int ItemsPerPage, int PageNumber)
        {
            if(minPrice < 0 || maxPrice < 0)
            {
                throw new ArgumentException("Prices cannot be negative !");
            }

            if(minPrice > maxPrice)
            {
                throw new ArgumentException("Minimum Price Cannot be Greater than Maximum Price");
            }

            if (PageNumber <= 0)
            {
                throw new ArgumentException("Page number must be greater than zero");
            }

            var products = (await _productRepository.GetProductsByPriceRange(minPrice, maxPrice))
                           .Where(p => p.IsDeleted != false)
                           .Skip(ItemsPerPage * (PageNumber - 1))
                           .Take(ItemsPerPage)
                           .ToList();

            var productsDtos = _mapper.Map <List<CreateOrUpdateProductDtos>>(products);
            var resultDataList = new ResultDataList<CreateOrUpdateProductDtos>()
            {
                Entities = productsDtos,
                Count = productsDtos.Count()
            };
            return resultDataList;
        }

        public async Task<ResultDataList<CreateOrUpdateProductDtos>> FilterNewlyAddedProductsAsync(int count, int ItemsPerPage, int PageNumber)
        {
            if (count <= 0)
            {
                throw new ArgumentException("The count must be greater than zero");
            }
            if (PageNumber <= 0)
            {
                throw new ArgumentException("Page number must be greater than zero");
            }

            var products = (await _productRepository.GetNewlyAddedProducts(count))
                           .Where(p=>p.IsDeleted != false)
                           .Skip(ItemsPerPage * (PageNumber - 1))
                           .Take(ItemsPerPage)
                           .ToList();

            var ProductsDto = _mapper.Map<List<CreateOrUpdateProductDtos>>(products);
            var resultDataLists = new ResultDataList<CreateOrUpdateProductDtos>()
            {
                Entities = ProductsDto,
                Count = ProductsDto.Count()
            };
            return resultDataLists;
        }

        public async Task<ResultDataList<CreateOrUpdateProductDtos>> FilterDiscountedProducts(int ItemsPerPage, int PageNumber)
        {
            if (PageNumber <= 0)
            {
                throw new ArgumentException("Page number must be greater than zero");
            }

            var products = (await _productRepository.GetDiscountedProducts())
                           .Where(p=>p.IsDeleted != false)
                           .Skip(ItemsPerPage * (PageNumber - 1))
                           .Take(ItemsPerPage)
                           .ToList();

            if(products is null)//!products.Any() =>empty ?
            {
                throw new ArgumentException("No discounted products found");
            }

            var productsDto = _mapper.Map<List<CreateOrUpdateProductDtos>>(products);
            var resultDataList = new ResultDataList<CreateOrUpdateProductDtos>()
            {
                Entities = productsDto,
                Count = productsDto.Count()
            };
            return resultDataList;
        }

        public async Task<ResultDataList<GetAllProductsForUserDto>> GetAllPaginationForUser(int ItemsPerPage, int PageNumber)
        {
            if (PageNumber <= 0)
            {
                throw new ArgumentException("Page number must be greater than zero");
            }

            var products = (await _productRepository.GetAllAsync())
                            .Where(p => p.IsDeleted != false)
                            .Skip(ItemsPerPage * (PageNumber - 1))
                            .Take(ItemsPerPage)
                            .Select(p => new GetAllProductsForUserDto
                            {
                                Id = p.Id,
                                Name = p.ModelName,
                                Description = p.Description,
                                Brand = p.Brand,
                                DateAdded = p.DateAdded,
                            }).ToList();

            var resultDataList = new ResultDataList<GetAllProductsForUserDto>()
            {
                Entities = products,
                Count = products.Count()
            };
            return resultDataList;
        }

        
    }
}
