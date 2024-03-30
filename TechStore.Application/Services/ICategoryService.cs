using TechStore.Dtos;
using TechStore.Dtos.CategoryDtos;
using TechStore.Dtos.ViewResult;

namespace TechStore.Application.Services
{
    public interface ICategoryService
    {
        Task<ResultView<CategorySpecificationDto>> CreateCategory(CategoryDto category,List<SpecificationsDto> specificationsDto);
        Task<ResultDataList<CategoryDto>> GetAllCategory();
        Task<CategorySpecificationDto> GetCategoryById(int id);
        Task<CategoryDto> GetCategoryByName(string name);
        Task<ResultView<CategoryDto>> HardDeleteCategory(CategoryDto category);
        Task<ResultView<CategoryDto>> SoftDeleteCategory(int id);
        Task<ResultView<CategoryDto>> UpdateCategory(CategoryDto category);
        //Task<ResultView<CategoryDto>> AddSpecificationCategory(CategoryDto category,List<SpecificationsDto> specificationsDto);
    }
}