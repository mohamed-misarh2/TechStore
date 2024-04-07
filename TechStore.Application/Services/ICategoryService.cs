using TechStore.Application.Contract;
using TechStore.Dtos.CategoryDtos;
using TechStore.Dtos.ProductDtos;
using TechStore.Dtos.ViewResult;

namespace TechStore.Application.Services
{
    public interface ICategoryService
    {
        Task<ResultView<CategorySpecificationDto>> CreateCategory(CategoryDto category,List<SpecificationsDto> specificationsDto);
        Task<ResultView<CategorySpecificationDto>> UpdateCategory(CategoryDto updatedcategory, List<SpecificationsDto> specificationsDtos);
        Task<ResultDataList<CategoryDto>> GetAllCategory();
        Task<CategorySpecificationDto> GetCategoryById(int id);
        Task<CategoryDto> GetCategoryByName(string name);
        Task<ResultView<CategoryDto>> HardDeleteCategory(int id);
        Task<ResultView<CategoryDto>> SoftDeleteCategory(int id);
        Task<ResultView<CategorySpecificationDto>> DeleteSpecFromCategory(int CategoryId, int SpecID);
        Task<ResultView<CategorySpecificationDto>> AddSpecToCategory(int CategoryId, SpecificationsDto specificationsDto);
        Task<ResultDataList<SpecificationsDto>> GetSpecificationsByCategoryId(int CategoryId);
        

            //Task<ResultView<CategoryDto>> AddSpecificationCategory(CategoryDto category,List<SpecificationsDto> specificationsDto);
        }
}