using OtoKurSkoda.Application.Defaults;
using OtoKurSkoda.Application.Dtos;

namespace OtoKurSkoda.Application.Services.UserServices.Interfaces
{
    public interface IAddressService
    {
        Task<ServiceResult> GetUserAddressesAsync(Guid userId);
        Task<ServiceResult> GetByIdAsync(Guid id, Guid userId);
        Task<ServiceResult> CreateAsync(Guid userId, CreateAddressRequest request);
        Task<ServiceResult> UpdateAsync(Guid userId, UpdateAddressRequest request);
        Task<ServiceResult> DeleteAsync(Guid id, Guid userId);
        Task<ServiceResult> SetDefaultAsync(Guid id, Guid userId);
    }
}
