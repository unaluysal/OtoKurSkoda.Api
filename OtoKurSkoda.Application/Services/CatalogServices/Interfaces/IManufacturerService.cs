using OtoKurSkoda.Application.Defaults;
using OtoKurSkoda.Application.Dtos;

namespace OtoKurSkoda.Application.Services.CatalogServices.Interfaces
{
    public interface IManufacturerService
    {
        Task<ServiceResult> GetAllAsync();
        Task<ServiceResult> GetActiveAsync();
        Task<ServiceResult> GetByIdAsync(Guid id);
        Task<ServiceResult> GetBySlugAsync(string slug);
        Task<ServiceResult> CreateAsync(CreateManufacturerRequest request);
        Task<ServiceResult> UpdateAsync(UpdateManufacturerRequest request);
        Task<ServiceResult> DeleteAsync(Guid id);
    }
}
