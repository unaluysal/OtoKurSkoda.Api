using OtoKurSkoda.Application.Defaults;
using OtoKurSkoda.Application.Dtos;

namespace OtoKurSkoda.Application.Services.VehicleServices.Interfaces
{
    public interface IBrandService
    {
        Task<ServiceResult> GetAllAsync();
        Task<ServiceResult> GetActiveAsync();
        Task<ServiceResult> GetByIdAsync(Guid id);
        Task<ServiceResult> GetBySlugAsync(string slug);
        Task<ServiceResult> CreateAsync(CreateBrandRequest request);
        Task<ServiceResult> UpdateAsync(UpdateBrandRequest request);
        Task<ServiceResult> DeleteAsync(Guid id);
    }
}
