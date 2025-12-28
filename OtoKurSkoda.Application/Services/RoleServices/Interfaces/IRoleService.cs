using OtoKurSkoda.Application.Defaults;
using OtoKurSkoda.Application.Dtos;

namespace OtoKurSkoda.Application.Services.RoleServices.Interfaces
{
    public interface IRoleService
    {
        Task<ServiceResult> GetAllAsync();
        Task<ServiceResult> GetByIdAsync(Guid id);
        Task<ServiceResult> CreateAsync(CreateRoleRequest request);
        Task<ServiceResult> UpdateAsync(UpdateRoleRequest request);
        Task<ServiceResult> DeleteAsync(Guid id);
    }
}