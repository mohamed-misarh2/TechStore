using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Application.Contract;
using TechStore.Dtos;
using TechStore.Dtos.CategoryDtos;
using TechStore.Dtos.ViewResult;
using TechStore.Models;

namespace TechStore.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IspecificationsRepository _specificationsRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository,IspecificationsRepository specificationsRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _specificationsRepository = specificationsRepository;
            _mapper = mapper;
        }

        public async Task<ResultView<CategorySpecificationDto>> CreateCategory(CategoryDto category, List<SpecificationsDto> specificationsDtos)
        {
            var allcategories = await _categoryRepository.GetAllAsync();
            var oldcat = allcategories.Where(c => c.Name == category.Name).FirstOrDefault();
            if (oldcat != null)
            {
                return new ResultView<CategorySpecificationDto> { Entity = null, IsSuccess = false, Message = "Already Exist" };
            }
            else
            {

                var cat = _mapper.Map<Category>(category);
                foreach (var specificationn in specificationsDtos)
                {
                    var specificationModel = _mapper.Map<Specification>(specificationn);
                    cat.CategorySpecifications.Add(new CategorySpecifications { Category = cat, Specification = specificationModel });

                }

                var newCat = await _categoryRepository.CreateAsync(cat);
                await _categoryRepository.SaveChangesAsync();
                var catDto = _mapper.Map<CategoryDto>(newCat);

                var CategorySpecificationDto = new CategorySpecificationDto { Category = catDto, SpecificationsDtos = specificationsDtos };

                return new ResultView<CategorySpecificationDto> { Entity = CategorySpecificationDto, IsSuccess = true, Message = "Created Successfully" };
            }
        }


        //public async Task<ResultView<CategorySpecificationDto>> CreateCategory(CategoryDto category, List<SpecificationsDto> specificationsDtos)
        //{
 
        //    var allcategories = await _categoryRepository.GetAllAsync();
        //    var oldcat = allcategories.Where(c => c.Name == category.Name).FirstOrDefault();
        //    if (oldcat != null)
        //    {
        //        return new ResultView<CategorySpecificationDto> { Entity = null, IsSuccess = false, Message = "Already Exist" };
        //    }

        //    var cat = _mapper.Map<Category>(category);

        //    foreach (var specificationDto in specificationsDtos)
        //    {
              
        //        var existingSpecification = await _specificationsRepository.SearchByName(specificationDto.Name);
        //        if (existingSpecification == null)
        //        {
        //            var specificationModel = _mapper.Map<Specification>(specificationDto);
        //            cat.CategorySpecifications.Add(new CategorySpecifications { Category = cat, Specification = specificationModel });
        //        }
        //        else
        //        {
        //            cat.CategorySpecifications.Add(new CategorySpecifications { Category = cat, SpecificationId = existingSpecification.Id });
        //        }
        //    }

        //    var newCat = await _categoryRepository.CreateAsync(cat);
        //    await _categoryRepository.SaveChangesAsync();
        //    var catDto = _mapper.Map<CategoryDto>(newCat);

        //    var categorySpecificationDto = new CategorySpecificationDto { Category = catDto, SpecificationsDtos = specificationsDtos };

        //    return new ResultView<CategorySpecificationDto> { Entity = categorySpecificationDto, IsSuccess = true, Message = "Created Successfully" };
        //}


        public async Task<ResultView<CategoryDto>> UpdateCategory(CategoryDto updatedcategory)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(updatedcategory.Id);
            if (existingCategory == null)
            {
                return new ResultView<CategoryDto> { Entity = null, IsSuccess = false, Message = "Category not found" };
            }
            _mapper.Map(updatedcategory, existingCategory);

            var updatedCat = await _categoryRepository.UpdateAsync(existingCategory);
            if (updatedCat == null)
            {
                return new ResultView<CategoryDto> { Entity = null, IsSuccess = false, Message = "Failed to update Category" };
            }
            await _categoryRepository.SaveChangesAsync();

            var catDto = _mapper.Map<CategoryDto>(updatedCat);

            return new ResultView<CategoryDto> { Entity = catDto, IsSuccess = true, Message = "Updated Successfully" };
        }

        public async Task<ResultView<CategoryDto>> HardDeleteCategory(CategoryDto category)
        {
            try
            {
                var existingCategory = await _categoryRepository.GetByIdAsync((int)category.Id);
                var mCat = _mapper.Map<Category>(existingCategory);
                var oldCat = _categoryRepository.DeleteAsync(mCat);
                await _categoryRepository.SaveChangesAsync();
                var catDto = _mapper.Map<CategoryDto>(oldCat);
                return new ResultView<CategoryDto> { Entity = catDto, IsSuccess = true, Message = "Deleted Successfully" };
            }
            catch (Exception ex)
            {
                return new ResultView<CategoryDto> { Entity = null, IsSuccess = false, Message = ex.Message };

            }
        }
        
        public async Task<ResultView<CategoryDto>> SoftDeleteCategory(int id)
        {
            try
            {
                var oldCat = (await _categoryRepository.GetAllAsync()).FirstOrDefault(b => b.Id == id);
                oldCat.IsDeleted = true;
                await _categoryRepository.SaveChangesAsync();

                //var catSpec = await _specificationsRepository.GetSpecificationsByCategory(category.Id);///******??
                //foreach(var specification in catSpec)
                //{
                //    specification.IsDeleted = true;
                //    await _specificationsRepository.SaveChangesAsync();
                //}

                var catDto = _mapper.Map<CategoryDto>(oldCat);
                return new ResultView<CategoryDto> { Entity = catDto, IsSuccess = true, Message = "Deleted Successfully" };
            }
            catch (Exception ex)
            {
                return new ResultView<CategoryDto> { Entity = null, IsSuccess = false, Message = ex.Message };

            }
        }

        public async Task<ResultDataList<CategoryDto>> GetAllCategory()
        {
            var categories = await _categoryRepository.GetAllAsync();

            var catDtos = categories.Where(c=>c.IsDeleted == false).Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
            }).ToList();

            ResultDataList<CategoryDto> result = new ResultDataList<CategoryDto>();
            result.Entities = catDtos;
            result.Count = catDtos.Count();
            return result;
        }

        public async Task<CategorySpecificationDto> GetCategoryById(int id)
        {
            var cat = await _categoryRepository.GetByIdAsync(id);
            var spec = await _specificationsRepository.GetSpecificationsByCategory(id);

            var catDto = _mapper.Map<CategoryDto>(cat);//*******  how to return categoryspecif??
            var SpecDto = _mapper.Map<List<SpecificationsDto>>(spec);
            var cateSpec = new CategorySpecificationDto { Category = catDto , SpecificationsDtos = SpecDto };

            return cateSpec;
        }

        public async Task<List<CategoryDto>> GetCategoryByName(string name)
        {
            var cat = await _categoryRepository.SearchByName(name);
            var catDto = _mapper.Map<List<CategoryDto>>(cat);
            return catDto;

        }

        //public async Task<ResultView<CategorySpecificationDto>> AddSpecificationCategory(CategoryDto category, List<SpecificationsDto> specificationsDto)
        //{
        //    var ExistingCategory = await _categoryRepository.GetByIdAsync(category.Id);
        //    if (ExistingCategory is null)
        //    {
        //        return new ResultView<CategorySpecificationDto> { Entity = null, IsSuccess = false, Message = "Category doesn't Exist" };
        //    }

        //    var AllSpec = (await _specificationsRepository.GetAllAsync()).ToList();
        //    foreach

        //}
   
    
    }
}
