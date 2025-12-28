using OtoKurSkoda.Application.Defaults;
using OtoKurSkoda.Application.Dtos;

namespace OtoKurSkoda.Application.Services.CatalogServices.Interfaces
{
    public interface ICategoryService
    {
        Task<ServiceResult> GetAllAsync();
        Task<ServiceResult> GetTreeAsync();
        Task<ServiceResult> GetRootCategoriesAsync();
        Task<ServiceResult> GetChildrenAsync(Guid parentId);
        Task<ServiceResult> GetByIdAsync(Guid id);
        Task<ServiceResult> GetBySlugAsync(string slug);
        Task<ServiceResult> CreateAsync(CreateCategoryRequest request);
        Task<ServiceResult> UpdateAsync(UpdateCategoryRequest request);
        Task<ServiceResult> DeleteAsync(Guid id);
    }
}
