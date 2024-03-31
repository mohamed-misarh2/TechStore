using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        private readonly IproductCategorySpecifications _productCategorySpecifications;
        private readonly IspecificationsRepository _ispecificationsRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductService(IProductRepository productRepository,IproductCategorySpecifications productCategorySpecifications,IspecificationsRepository ispecificationsRepository,IMapper mapper,IWebHostEnvironment webHostEnvironment) {
            _productRepository = productRepository;
            _productCategorySpecifications = productCategorySpecifications;
            _ispecificationsRepository = ispecificationsRepository;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }


        //images
        private async Task<List<string>> SaveProductImages(List<IFormFile> images)
        {
            var imagePaths = new List<string>();

            // Loop through each uploaded image file
            foreach (var image in images)
            {
                if (image != null && image.Length > 0)
                {
                    // Generate unique file name
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);

                    // Specify the directory where images will be saved (e.g., wwwroot/images)
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "ImageProduct", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    imagePaths.Add("/ImageProduct/" + fileName); 
                    
                }
            }

            return imagePaths;
        }


        //admin
        public async Task<ResultView<ProductCategorySpecificationsListDto>> Create(CreateOrUpdateProductDtos productDto,List<ProductCategorySpecificationsDto> ProductCategorySpecificationsDto)
        {
            var OldProduct = (await _productRepository.GetAllAsync())
                             .Where(p => p.Description == productDto.Description).FirstOrDefault();
            if (OldProduct != null)
            {
                return new ResultView<ProductCategorySpecificationsListDto>
                {
                    Entity = null,
                    IsSuccess = false,
                    Message = "Product Already Exists !"
                };
            }

            var product = _mapper.Map<Product>(productDto);
            var imagePaths = await SaveProductImages(productDto.Images);

            // Set the image paths in the product entity
            product.Images = new List<Image>(); // Create new list for images
            foreach (var imagePath in imagePaths)
            {
                product.Images.Add(new Image { Name = imagePath }); // Add each image path to the list
            }

            var AddedProduct = await _productRepository.CreateAsync(product);
            await _productRepository.SaveChangesAsync();

            var list = new List<ProductCategorySpecifications>();

            foreach (var item in ProductCategorySpecificationsDto)
            {
                var itemModel = _mapper.Map<ProductCategorySpecifications>(item);
                itemModel.ProductId = AddedProduct.Id;
                itemModel.CategoryId = AddedProduct.CategoryId;
                var res = await _productCategorySpecifications.CreateAsync(itemModel);
                list.Add(res);
            }

            await _productCategorySpecifications.SaveChangesAsync();
            
            var listDto = _mapper.Map<List<ProductCategorySpecificationsDto>>(list);

            var NewProductDto = _mapper.Map<CreateOrUpdateProductDtos>(product);
            var ProductCategorySpecificationsList = new ProductCategorySpecificationsListDto
            {
                CreateOrUpdateProductDtos = NewProductDto,
                ProductCategorySpecifications = listDto
            };

            return new ResultView<ProductCategorySpecificationsListDto>
            {
                Entity = ProductCategorySpecificationsList,
                IsSuccess = true,
                Message = "Product Added Successfully !"
            };
        }
        
        public async Task<ResultView<GetProductSpecificationNameValueDtos>> GetOne(int id)
        {
            var ProductModel = await _productRepository.GetProductWithImages(id);
            if (ProductModel != null)
            {
                var productDto = _mapper.Map<GetAllProductsDtos>(ProductModel);
                productDto.Images = ProductModel.Images.Select(image => image.Name).ToList();


                var productCategorySpecificationsList = (await _productCategorySpecifications.GetProductCategorySpecifications(id)).ToList();

                var SpecList = new List<GetSpecificationsNameValueDtos>();

                foreach(var productCatSpec in productCategorySpecificationsList)
                {
                    var SpecName = await _ispecificationsRepository.GetSpecificationNameById((int)productCatSpec.SpecificationId);
                    SpecList.Add(new GetSpecificationsNameValueDtos { Name = SpecName, Value = productCatSpec.Value });
                }

                var getAll = new GetProductSpecificationNameValueDtos { productsDtos = productDto, specificationsNameValueDtos = SpecList };

                return new ResultView<GetProductSpecificationNameValueDtos>
                {
                    Entity = getAll,
                    IsSuccess = true,
                    Message = "Product Retrived Successfully !"
                };
            }

            return new ResultView<GetProductSpecificationNameValueDtos>
            {
                Entity = null,
                IsSuccess = false,
                Message = "Product doesn't Exist !"
            };

        }

        public async Task<ResultView<ProductCategorySpecificationsListDto>> Update(CreateOrUpdateProductDtos productDto , List<ProductCategorySpecificationsDto> ProductCategorySpecificationsDto)
        {
            //var OldProduct = await _productRepository.GetByIdAsync(productDto.Id);//error here tracking id

            var OldProduct = await _productRepository.GetByIdAsync(productDto.Id);
            await _productRepository.DetachEntityAsync(OldProduct);

            if (OldProduct != null)
            {
                var updatedProduct = _mapper.Map<Product>(productDto);
                var NewUpdatedProduct = await _productRepository.UpdateAsync(updatedProduct);
                await _productRepository.SaveChangesAsync();

                List<ProductCategorySpecifications> productCategorySpecifications = new List<ProductCategorySpecifications>();

                foreach (var productSpecDto in ProductCategorySpecificationsDto)
                {
                    var updatedSpecModel = _mapper.Map<ProductCategorySpecifications>(productSpecDto);
                    var updatedSpec = await _productCategorySpecifications.UpdateAsync(updatedSpecModel);
                    productCategorySpecifications.Add(updatedSpec);
                }

                await _productCategorySpecifications.SaveChangesAsync();
                var NewUpdatedProductDto = _mapper.Map<CreateOrUpdateProductDtos>(NewUpdatedProduct);
                var productCategorySpecDto = _mapper.Map<List<ProductCategorySpecificationsDto>>(productCategorySpecifications);

                var productCategorySpecListDto = new ProductCategorySpecificationsListDto
                {
                    CreateOrUpdateProductDtos = NewUpdatedProductDto,
                    ProductCategorySpecifications = productCategorySpecDto
                };

                return new ResultView<ProductCategorySpecificationsListDto>
                {
                    Entity = productCategorySpecListDto,
                    IsSuccess = true,
                    Message = "Product Updated Successfully !"
                };

            }

            return new ResultView<ProductCategorySpecificationsListDto>
            {
                Entity = null,
                IsSuccess = false,
                Message = "Product doesn't Exist & failed To Update !"
            };
        }

        public async Task<ResultView<ProductCategorySpecificationsListDto>> SoftDelete(int productId)
        {
            var OldProduct = (await _productRepository.GetByIdAsync(productId));
            if (OldProduct != null)
            {
                OldProduct.IsDeleted = true;
                await _productRepository.SaveChangesAsync();

                var ProductCatSpec = (await _productCategorySpecifications.GetProductCategorySpecifications(productId)).ToList();

                foreach(var productCategorySpec in ProductCatSpec)
                {
                    productCategorySpec.IsDeleted = true;
                }
                await _productCategorySpecifications.SaveChangesAsync();

                var DeletedProductDto = _mapper.Map<CreateOrUpdateProductDtos>(OldProduct);
                var ProductCatSpecDtos = _mapper.Map<List<ProductCategorySpecificationsDto>>(ProductCatSpec);

                var ProductCatSpecListDtos = new ProductCategorySpecificationsListDto { CreateOrUpdateProductDtos = DeletedProductDto, ProductCategorySpecifications = ProductCatSpecDtos };

                return new ResultView<ProductCategorySpecificationsListDto>
                {
                    Entity = ProductCatSpecListDtos,
                    IsSuccess = true,
                    Message = "Product Deleted Successfully !"

                };
            }

            return new ResultView<ProductCategorySpecificationsListDto>
            {
                Entity = null,
                IsSuccess = false,
                Message = "failed to delete this product !"

            };

        }
        
        public async Task<ResultView<ProductCategorySpecificationsListDto>> HardDelete(int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product != null)
            {

                var ProductCatSpec = (await _productCategorySpecifications.GetProductCategorySpecifications(productId)).ToList();

                var productCategorySpecificationsList = new List<ProductCategorySpecifications>();
                foreach (var productCatSpec in ProductCatSpec)
                {
                    var DeletedproductCategorySpecifications = await _productCategorySpecifications.DeleteAsync(productCatSpec);
                    productCategorySpecificationsList.Add(DeletedproductCategorySpecifications);
                }

                await _productCategorySpecifications.SaveChangesAsync();

                var DeletedProductModel = await _productRepository.DeleteAsync(product);
                await _productRepository.SaveChangesAsync();

                var DeletedProductDto = _mapper.Map<CreateOrUpdateProductDtos>(DeletedProductModel);
                var productCategorySpecificationsListtDtos = _mapper.Map<List<ProductCategorySpecificationsDto>>(productCategorySpecificationsList);

                var productCategorySpecificationsListDto = new ProductCategorySpecificationsListDto
                {
                    CreateOrUpdateProductDtos = DeletedProductDto,
                    ProductCategorySpecifications = productCategorySpecificationsListtDtos
                };

                return new ResultView<ProductCategorySpecificationsListDto>
                {
                        Entity = productCategorySpecificationsListDto,
                        IsSuccess = true,
                        Message = "Product Deleted Sucessfully"
                };
            }

            return new ResultView<ProductCategorySpecificationsListDto>
            {
                Entity = null,
                IsSuccess = false,
                Message = "Product Not Found"
            };
        }

        public async Task<ResultDataList<GetAllProductsDtos>> GetAllPagination(int ItemsPerPage, int PageNumber)
        {

            if (PageNumber > 1)
            {
                var products = (await _productRepository.GetAllAsync())
                            .Where(p => p.IsDeleted == false)
                            .Skip(ItemsPerPage * PageNumber ).Take(ItemsPerPage)
                            .Select(p => new GetAllProductsDtos
                            {
                                Id = p.Id,
                                ModelName = p.ModelName,
                                Description = p.Description,
                                Brand = p.Brand,
                                CategoryId = p.CategoryId,
                                DateAdded = p.DateAdded,
                                //FullName = $"{p.User.FirstName} {p.User.LastName}",
                                IsDeleted = p.IsDeleted
                            }).ToList();

                var resultDataList = new ResultDataList<GetAllProductsDtos>()
                {
                    Entities = products,
                    Count = products.Count()
                };
            }

            var resultDataListt = new ResultDataList<GetAllProductsDtos>()
            {
                Entities = null,
                Count = 0
            };
            return resultDataListt;
     }




        //user

        public async Task<ResultDataList<GetAllProductsDtos>> FilterProductsByCategory(int categoryId, int ItemsPerPage, int PageNumber)
        {
            try
            {
                if (categoryId <= 0)
                {
                    throw new ArgumentException("Invalid CategoryId");
                }

                if (PageNumber <= 0)
                {
                    throw new ArgumentException("Page number must be greater than zero");
                }

                var products = (await _productRepository.GetProductsByCategory(categoryId))
                               .Where(p => p.IsDeleted == false)
                               .Skip(ItemsPerPage * (PageNumber - 1))
                               .Take(ItemsPerPage)
                               .Select(p => new GetAllProductsDtos
                               {
                                    Id = p.Id,
                                    ModelName = p.ModelName,
                                    Description = p.Description,
                                    Brand = p.Brand,
                                    CategoryId = p.CategoryId,
                                    DateAdded = p.DateAdded,
                                    Price = p.Price,
                                    DiscountValue = p.DiscountValue,
                                    DiscountedPrice = p.Price - (p.Price * p.DiscountValue / 100),
                                    IsDeleted = p.IsDeleted,
                                    Images = p.Images.Select(i=>i.Name).ToList()
                                    
                               }).ToList();

                var ProductsDto = _mapper.Map<List<GetAllProductsDtos>>(products);
             
                var resultDataList = new ResultDataList<GetAllProductsDtos>()
                {
                    Entities = ProductsDto,
                    Count = ProductsDto.Count()
                };
                return resultDataList;
            }
            catch (Exception ex)
            {
                var resultDataList = new ResultDataList<GetAllProductsDtos>()
                {
                    Entities = null,
                    Count = 0
                };
                return resultDataList;
            }

        }

        //sort 
        public async Task<ResultDataList<GetAllProductsDtos>> SortProductsByDesending()
        {
            var products = (await _productRepository.GetProductsByDescending()).ToList();
            var productsDto = _mapper.Map<List<GetAllProductsDtos>>(products);
            ResultDataList<GetAllProductsDtos> res;
            if(products != null)
            {
                res = new ResultDataList<GetAllProductsDtos>()
                {
                    Entities = productsDto,
                    Count = products.Count()
                };
            }
            else
            {
                res = new ResultDataList<GetAllProductsDtos>()
                {
                    Entities = null,
                    Count = 0
                };
            }
            return res;
        }


        public async Task<ResultDataList<GetAllProductsDtos>> SortProductsByAscending()
        {
            var products = (await _productRepository.GetProductsByAscending()).ToList();
            var productsDto = _mapper.Map<List<GetAllProductsDtos>>(products);
            ResultDataList<GetAllProductsDtos> res;
            if(products != null)
            {
                res = new ResultDataList<GetAllProductsDtos>()
                {
                    Entities = productsDto,
                    Count = products.Count()
                };
            }
            else
            {
                res = new ResultDataList<GetAllProductsDtos>()
                {
                    Entities = null,
                    Count = 0
                };
            }

            
            return res;
        }

        //search
        public async Task<ResultDataList<GetAllProductsDtos>> SearchProduct(string Name, int ItemsPerPage, int PageNumber)
        {
            try
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
                               .Where(p => p.IsDeleted == false)
                               .Skip(ItemsPerPage * (PageNumber - 1)).Take(ItemsPerPage)
                                .Select(p => new GetAllProductsDtos
                                {
                                    Id = p.Id,
                                    ModelName = p.ModelName,
                                    Description = p.Description,
                                    Brand = p.Brand,
                                    CategoryId = p.CategoryId,
                                    DateAdded = p.DateAdded,
                                    Price = p.Price,
                                    DiscountValue = p.DiscountValue,
                                    DiscountedPrice = p.Price - (p.Price * p.DiscountValue / 100),
                                    IsDeleted = p.IsDeleted
                                }).ToList();

                var ProductsDto = _mapper.Map<List<GetAllProductsDtos>>(products);
                var resultDataList = new ResultDataList<GetAllProductsDtos>()
                {
                    Entities = ProductsDto,
                    Count = ProductsDto.Count()
                };
                return resultDataList;
            }
            catch(Exception ex)
            {
                var resultDataList = new ResultDataList<GetAllProductsDtos>()
                {
                    Entities = null,
                    Count = 0
                };
                return resultDataList;
            }
            
        }

       


        //filter  
       

        //public async Task<ResultDataList<CreateOrUpdateProductDtos>> FiltertRelatedProducts(int productId, int ItemsPerPage, int PageNumber)
        //{
        //    try
        //    {
        //        if (PageNumber <= 0)
        //        {
        //            throw new ArgumentException("Page number must be greater than zero");
        //        }

        //        var product = await _productRepository.GetByIdAsync(productId);
        //        var products = (await _productRepository.GetRelatedProducts(product))
        //                       .Where(p => p.IsDeleted != false)
        //                       .Skip(ItemsPerPage * (PageNumber - 1))
        //                       .Take(ItemsPerPage)
        //                       .ToList();

        //        var ProductsDto = _mapper.Map<List<CreateOrUpdateProductDtos>>(products);
        //        var resultDataList = new ResultDataList<CreateOrUpdateProductDtos>()
        //        {
        //            Entities = ProductsDto,
        //            Count = ProductsDto.Count()
        //        };
        //        return resultDataList;
        //    }
        //    catch (Exception ex) 
        //    {
        //        var resultDataList = new ResultDataList<CreateOrUpdateProductDtos>()
        //        {
        //            Entities = null,
        //            Count = 0
        //        };
        //        return resultDataList;
        //    }
            
        //}

        //public async Task<ResultDataList<CreateOrUpdateProductDtos>> FilterProductsByPriceRange(decimal minPrice, decimal maxPrice, int ItemsPerPage, int PageNumber)
        //{
        //    try
        //    {
        //        if (minPrice < 0 || maxPrice < 0)
        //        {
        //            throw new ArgumentException("Prices cannot be negative !");
        //        }

        //        if (minPrice > maxPrice)
        //        {
        //            throw new ArgumentException("Minimum Price Cannot be Greater than Maximum Price");
        //        }

        //        if (PageNumber <= 0)
        //        {
        //            throw new ArgumentException("Page number must be greater than zero");
        //        }

        //        var products = (await _productRepository.GetProductsByPriceRange(minPrice, maxPrice))
        //                       .Where(p => p.IsDeleted != false)
        //                       .Skip(ItemsPerPage * (PageNumber - 1))
        //                       .Take(ItemsPerPage)
        //                       .ToList();

        //        var productsDtos = _mapper.Map<List<CreateOrUpdateProductDtos>>(products);
        //        var resultDataList = new ResultDataList<CreateOrUpdateProductDtos>()
        //        {
        //            Entities = productsDtos,
        //            Count = productsDtos.Count()
        //        };
        //        return resultDataList;
        //    }
        //    catch (Exception ex) 
        //    {
        //        var resultDataList = new ResultDataList<CreateOrUpdateProductDtos>()
        //        {
        //            Entities = null,
        //            Count = 0
        //        };
        //        return resultDataList;
        //    }

            
            
        //}

        //public async Task<ResultDataList<CreateOrUpdateProductDtos>> FilterNewlyAddedProducts(int count, int ItemsPerPage, int PageNumber)
        //{
        //    try
        //    {
        //        if (count <= 0)
        //        {
        //            throw new ArgumentException("The count must be greater than zero");
        //        }
        //        if (PageNumber <= 0)
        //        {
        //            throw new ArgumentException("Page number must be greater than zero");
        //        }

        //        var products = (await _productRepository.GetNewlyAddedProducts(count))
        //                       .Where(p => p.IsDeleted != false)
        //                       .Skip(ItemsPerPage * (PageNumber - 1))
        //                       .Take(ItemsPerPage)
        //                       .ToList();

        //        var ProductsDto = _mapper.Map<List<CreateOrUpdateProductDtos>>(products);
        //        var resultDataLists = new ResultDataList<CreateOrUpdateProductDtos>()
        //        {
        //            Entities = ProductsDto,
        //            Count = ProductsDto.Count()
        //        };
        //        return resultDataLists;
        //    }
        //    catch(Exception ex) 
        //    {
        //        var resultDataLists = new ResultDataList<CreateOrUpdateProductDtos>()
        //        {
        //            Entities = null,
        //            Count = 0
        //        };
        //        return resultDataLists;
        //    }

            
        //}

        //public async Task<ResultDataList<CreateOrUpdateProductDtos>> FilterDiscountedProducts(int ItemsPerPage, int PageNumber)
        //{
        //    try
        //    {
        //        if (PageNumber <= 0)
        //        {
        //            throw new ArgumentException("Page number must be greater than zero");
        //        }

        //        var products = (await _productRepository.GetDiscountedProducts())
        //                       .Where(p => p.IsDeleted != false)
        //                       .Skip(ItemsPerPage * (PageNumber - 1))
        //                       .Take(ItemsPerPage)
        //                       .ToList();

        //        if (products is null)
        //        {
        //            throw new ArgumentException("No discounted products found");
        //        }

        //        var productsDto = _mapper.Map<List<CreateOrUpdateProductDtos>>(products);
        //        var resultDataList = new ResultDataList<CreateOrUpdateProductDtos>()
        //        {
        //            Entities = productsDto,
        //            Count = productsDto.Count()
        //        };
        //        return resultDataList;
        //    }
        //    catch (Exception ex) 
        //    {
        //        var resultDataList = new ResultDataList<CreateOrUpdateProductDtos>()
        //        {
        //            Entities = null,
        //            Count = 0
        //        };
        //        return resultDataList;
        //    }
            
        //}

        //public Task<ResultDataList<CreateOrUpdateProductDtos>> FilterProductsByWarranty(string Warranty, int ItemsPerPage, int PageNumber)
        //{
        //    throw new NotImplementedException();
        //}


        public async Task<ResultDataList<GetAllProductsDtos>> FilterProducts(FillterProductsDtos fillterProductsDto)
        {
            var products = (await _productRepository.FilterProducts(fillterProductsDto)).ToList();
            var productsDto = _mapper.Map<List<GetAllProductsDtos>>(products);
            ResultDataList<GetAllProductsDtos> resultDataList;
            if(products != null)
            {
                resultDataList = new ResultDataList<GetAllProductsDtos>()
                {
                    Entities = productsDto,
                    Count = productsDto.Count()
                };
            }
            else
            {
                resultDataList = new ResultDataList<GetAllProductsDtos>()
                {
                    Entities = null,
                    Count = 0
                };
            }
            
            return resultDataList;
        }
        public async Task<List<string>> GetBrands()
        {
            var brands = await _productRepository.GetBrands();
            return brands;
        }
    }
}
