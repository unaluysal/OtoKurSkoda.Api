using OtoKurSkoda.Application.Defaults;
using OtoKurSkoda.Application.Dtos;

namespace OtoKurSkoda.Application.Services.VehicleServices.Interfaces
{
    public interface IVehicleModelService
    {
        Task<ServiceResult> GetAllAsync();
        Task<ServiceResult> GetByBrandIdAsync(Guid brandId);
        Task<ServiceResult> GetByIdAsync(Guid id);
        Task<ServiceResult> GetBySlugAsync(string brandSlug, string modelSlug);
        Task<ServiceResult> CreateAsync(CreateVehicleModelRequest request);
        Task<ServiceResult> UpdateAsync(UpdateVehicleModelRequest request);
        Task<ServiceResult> DeleteAsync(Guid id);
    }
}
