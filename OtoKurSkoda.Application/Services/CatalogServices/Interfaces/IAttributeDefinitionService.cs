using OtoKurSkoda.Application.Defaults;
using OtoKurSkoda.Application.Dtos;

namespace OtoKurSkoda.Application.Services.CatalogServices.Interfaces
{
    public interface IAttributeDefinitionService
    {
        Task<ServiceResult> GetAllAsync();
        Task<ServiceResult> GetByIdAsync(Guid id);
        Task<ServiceResult> GetByCategoryIdAsync(Guid categoryId);
        Task<ServiceResult> GetFilterableAsync();
        Task<ServiceResult> CreateAsync(CreateAttributeDefinitionRequest request);
        Task<ServiceResult> UpdateAsync(UpdateAttributeDefinitionRequest request);
        Task<ServiceResult> DeleteAsync(Guid id);
        Task<ServiceResult> AssignToCategoryAsync(Guid categoryId, Guid attributeId, bool isRequired = false);
        Task<ServiceResult> RemoveFromCategoryAsync(Guid categoryId, Guid attributeId);
    }
}
