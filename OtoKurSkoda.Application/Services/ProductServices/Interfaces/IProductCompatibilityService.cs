using OtoKurSkoda.Application.Defaults;
using OtoKurSkoda.Application.Dtos;

namespace OtoKurSkoda.Application.Services.ProductServices.Interfaces
{
    public interface IProductCompatibilityService
    {
        Task<ServiceResult> GetByProductIdAsync(Guid productId);
        Task<ServiceResult> GetProductsByVehicleAsync(Guid vehicleGenerationId);
        Task<ServiceResult> AddCompatibilityAsync(AddProductCompatibilityRequest request);
        Task<ServiceResult> RemoveCompatibilityAsync(Guid id);
        Task<ServiceResult> SetCompatibilitiesAsync(SetProductCompatibilitiesRequest request);
    }
}
