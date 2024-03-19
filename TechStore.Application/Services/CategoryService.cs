using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Application.Contract;
using TechStore.Dtos.CategoryDtos;
using TechStore.Dtos.ViewResult;
using TechStore.Models;

namespace TechStore.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<ResultView<CategoryDto>> CreateCategory(CategoryDto category)
        {
            var allcategories = await _categoryRepository.GetAllAsync();
            var oldcat = allcategories.Where(c => c.Name == category.Name).FirstOrDefault();
            if (oldcat != null)
            {
                return new ResultView<CategoryDto> { Entity = null, IsSuccess = false, Message = "Already Exist" };
            }
            else
            {
                var cat = _mapper.Map<Category>(category);
                var newCat = await _categoryRepository.CreateAsync(cat);
                await _categoryRepository.SaveChangesAsync();
                var catDto = _mapper.Map<CategoryDto>(newCat);
                return new ResultView<CategoryDto> { Entity = catDto, IsSuccess = true, Message = "Created Successfully" };
            }
        }

        public async Task<ResultView<CategoryDto>> UpdateCategory(CategoryDto updatedcategory)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync((int)updatedcategory.Id);
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
        public async Task<ResultView<CategoryDto>> SoftDeleteCategory(CategoryDto category)
        {
            try
            {
                var mCat = _mapper.Map<Category>(category);
                var oldCat = (await _categoryRepository.GetAllAsync()).FirstOrDefault(b => b.Id == category.Id);
                oldCat.IsDeleted = true;
                await _categoryRepository.SaveChangesAsync();
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

            var catDtos = categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
            }).ToList();

            ResultDataList<CategoryDto> result = new ResultDataList<CategoryDto>();
            result.Entities = catDtos;
            return result;
        }

        public async Task<CategoryDto> GetCategoryById(int id)
        {
            var cat = await _categoryRepository.GetByIdAsync(id);
            var catDto = _mapper.Map<CategoryDto>(cat);
            return catDto;
        }
        public async Task<CategoryDto> GetCategoryByName(string name)
        {
            var cat = await _categoryRepository.SearchByName(name);
            var catDto = _mapper.Map<CategoryDto>(cat);
            return catDto;

        }
    }
}
