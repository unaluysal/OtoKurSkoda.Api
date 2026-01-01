using OtoKurSkoda.Application.Defaults;
using OtoKurSkoda.Application.Dtos;

namespace OtoKurSkoda.Application.Services.UserServices.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResult> GetAllAsync();
        Task<ServiceResult> GetByIdAsync(Guid id);
        Task<ServiceResult> GetByIdWithRolesAsync(Guid id);
        Task<ServiceResult> CreateAsync(CreateUserRequest request);
        Task<ServiceResult> UpdateAsync(UpdateUserRequest request);
        Task<ServiceResult> DeleteAsync(Guid id);
        Task<ServiceResult> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
    }
}
