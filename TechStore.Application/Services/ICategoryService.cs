using TechStore.Dtos.CategoryDtos;
using TechStore.Dtos.ViewResult;

namespace TechStore.Application.Services
{
    public interface ICategoryService
    {
        Task<ResultView<CategoryDto>> CreateCategory(CategoryDto category);
        Task<ResultDataList<CategoryDto>> GetAllCategory();
        Task<CategoryDto> GetCategoryById(int id);
        Task<CategoryDto> GetCategoryByName(string name);
        Task<ResultView<CategoryDto>> HardDeleteCategory(CategoryDto category);
        Task<ResultView<CategoryDto>> SoftDeleteCategory(CategoryDto category);
        Task<ResultView<CategoryDto>> UpdateCategory(CategoryDto updatedcategory);
    }
}