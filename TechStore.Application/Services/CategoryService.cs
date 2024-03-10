using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Application.Contract;
using TechStore.Dtos.Category;
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
        public async Task<ResultView<CreateOrUpdateCategory>> Create(CreateOrUpdateCategory category)
        {
            var allcategories = await _categoryRepository.GetAllAsync();
            var oldcat = allcategories.Where(c => c.Name == category.Name).FirstOrDefault();
            if (oldcat != null)
            {
                return new ResultView<CreateOrUpdateCategory> { Entity = null, IsSuccess = false, Message = "Already Exist" };
            }
            else
            {
                var cat = _mapper.Map<Category>(category);
                var newCat = await _categoryRepository.CreateAsync(cat);
                await _categoryRepository.SaveChangesAsync();
                var catDto = _mapper.Map<CreateOrUpdateCategory>(newCat);
                return new ResultView<CreateOrUpdateCategory> { Entity = catDto, IsSuccess = true, Message = "Created Successfully" };
            }
        }

        public async Task<ResultView<CreateOrUpdateCategory>> Update(CreateOrUpdateCategory updatedcategory)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(updatedcategory.Id);
            if (existingCategory == null)
            {
                return new ResultView<CreateOrUpdateCategory> { Entity = null, IsSuccess = false, Message = "Category not found" };
            }
            _mapper.Map(updatedcategory, existingCategory);

            var updatedCat = await _categoryRepository.UpdateAsync(existingCategory);
            if (updatedCat == null)
            {
                return new ResultView<CreateOrUpdateCategory> { Entity = null, IsSuccess = false, Message = "Failed to update Category" };
            }
            await _categoryRepository.SaveChangesAsync();

            var catDto = _mapper.Map<CreateOrUpdateCategory>(updatedCat);
            return new ResultView<CreateOrUpdateCategory> { Entity = catDto, IsSuccess = true, Message = "Updated Successfully" };
        }
        public async Task<ResultView<CreateOrUpdateCategory>> HardDelete(CreateOrUpdateCategory category)
        {
            try
            {
                var existingCategory = await _categoryRepository.GetByIdAsync(category.Id);
                var mCat = _mapper.Map<Category>(existingCategory);
                var oldCat = _categoryRepository.DeleteAsync(mCat);
                await _categoryRepository.SaveChangesAsync();
                var catDto = _mapper.Map<CreateOrUpdateCategory>(oldCat);
                return new ResultView<CreateOrUpdateCategory> { Entity = catDto, IsSuccess = true, Message = "Deleted Successfully" };
            }
            catch (Exception ex)
            {
                return new ResultView<CreateOrUpdateCategory> { Entity = null, IsSuccess = false, Message = ex.Message };

            }
        }
        public async Task<ResultView<CreateOrUpdateCategory>> SoftDelete(CreateOrUpdateCategory category)
        {
            try
            {
                var mCat = _mapper.Map<Category>(category);
                var oldCat = (await _categoryRepository.GetAllAsync()).FirstOrDefault(b => b.Id == category.Id);
                oldCat.IsDeleted = true;
                await _categoryRepository.SaveChangesAsync();
                var catDto = _mapper.Map<CreateOrUpdateCategory>(oldCat);
                return new ResultView<CreateOrUpdateCategory> { Entity = catDto, IsSuccess = true, Message = "Deleted Successfully" };
            }
            catch (Exception ex)
            {
                return new ResultView<CreateOrUpdateCategory> { Entity = null, IsSuccess = false, Message = ex.Message };

            }
        }

        public async Task<ResultDataList<GetAllCategory>> GetAll()
        {
            var categories = await _categoryRepository.GetAllAsync();

            var catDtos = categories.Select(c => new GetAllCategory
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
            }).ToList();

            ResultDataList<GetAllCategory> result = new ResultDataList<GetAllCategory>();
            result.Entities = catDtos;
            return result;
        }

        public async Task<CreateOrUpdateCategory> GetById(int id)
        {
            var cat = await _categoryRepository.GetByIdAsync(id);
            var catDto = _mapper.Map<CreateOrUpdateCategory>(cat);
            return catDto;
        }
        public async Task<CreateOrUpdateCategory> GetByName(string name)
        {
            var cat = await _categoryRepository.GetByName(name);
            var catDto = _mapper.Map<CreateOrUpdateCategory>(cat);
            return catDto;

        }
    }
}
