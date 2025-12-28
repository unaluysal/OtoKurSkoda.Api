using OtoKurSkoda.Application.Defaults;
using OtoKurSkoda.Application.Dtos;

namespace OtoKurSkoda.Application.Services.UserServices.Interfaces
{
    public interface IUserRoleService
    {
        Task<ServiceResult> GetUserRoleGroupsAsync(Guid userId);
        Task<ServiceResult> GetUserPermissionsAsync(Guid userId);
        Task<ServiceResult> AssignRoleGroupAsync(AssignRoleGroupToUserRequest request);
        Task<ServiceResult> AssignMultipleRoleGroupsAsync(AssignMultipleRoleGroupsToUserRequest request);
        Task<ServiceResult> RemoveRoleGroupAsync(Guid userId, Guid roleGroupId);
        Task<ServiceResult> RemoveAllRoleGroupsAsync(Guid userId);
    }
}
