using TechStore.Dtos.Category;
using TechStore.Dtos.ViewResult;

namespace TechStore.Application.Services
{
    public interface ICategoryService
    {
        Task<ResultView<CreateOrUpdateCategory>> Create(CreateOrUpdateCategory category);
        Task<ResultDataList<GetAllCategory>> GetAll();
        Task<CreateOrUpdateCategory> GetById(int id);
        Task<CreateOrUpdateCategory> GetByName(string name);
        Task<ResultView<CreateOrUpdateCategory>> HardDelete(CreateOrUpdateCategory category);
        Task<ResultView<CreateOrUpdateCategory>> SoftDelete(CreateOrUpdateCategory category);
        Task<ResultView<CreateOrUpdateCategory>> Update(CreateOrUpdateCategory updatedcategory);
    }
}