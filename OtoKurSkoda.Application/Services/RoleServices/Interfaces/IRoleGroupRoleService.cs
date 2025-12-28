using OtoKurSkoda.Application.Defaults;
using OtoKurSkoda.Application.Dtos;

namespace OtoKurSkoda.Application.Services.RoleServices.Interfaces
{
    public interface IRoleGroupRoleService
    {
        Task<ServiceResult> GetRolesByGroupIdAsync(Guid roleGroupId);
        Task<ServiceResult> AssignRoleAsync(AssignRoleRequest request);
        Task<ServiceResult> AssignMultipleRolesAsync(AssignMultipleRolesRequest request);
        Task<ServiceResult> RemoveRoleAsync(Guid roleGroupId, Guid roleId);
        Task<ServiceResult> RemoveAllRolesAsync(Guid roleGroupId);
    }
}
