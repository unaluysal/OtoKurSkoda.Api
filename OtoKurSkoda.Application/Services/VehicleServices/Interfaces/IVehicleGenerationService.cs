using OtoKurSkoda.Application.Defaults;
using OtoKurSkoda.Application.Dtos;

namespace OtoKurSkoda.Application.Services.VehicleServices.Interfaces
{
    public interface IVehicleGenerationService
    {
        Task<ServiceResult> GetAllAsync();
        Task<ServiceResult> GetByModelIdAsync(Guid modelId);
        Task<ServiceResult> GetByIdAsync(Guid id);
        Task<ServiceResult> CreateAsync(CreateVehicleGenerationRequest request);
        Task<ServiceResult> UpdateAsync(UpdateVehicleGenerationRequest request);
        Task<ServiceResult> DeleteAsync(Guid id);
    }
}
