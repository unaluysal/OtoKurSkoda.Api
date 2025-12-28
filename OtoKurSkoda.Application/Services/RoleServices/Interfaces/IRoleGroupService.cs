using OtoKurSkoda.Application.Defaults;
using OtoKurSkoda.Application.Dtos;

namespace OtoKurSkoda.Application.Services.RoleServices.Interfaces
{
    public interface IRoleGroupService
    {
        Task<ServiceResult> GetAllAsync();
        Task<ServiceResult> GetByIdAsync(Guid id);
        Task<ServiceResult> GetWithRolesAsync(Guid id);
        Task<ServiceResult> CreateAsync(CreateRoleGroupRequest request);
        Task<ServiceResult> UpdateAsync(UpdateRoleGroupRequest request);
        Task<ServiceResult> DeleteAsync(Guid id);
    }
}