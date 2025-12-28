using OtoKurSkoda.Application.Defaults;
using OtoKurSkoda.Application.Dtos;

namespace OtoKurSkoda.Application.Services.AuthServices.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResult> RegisterAsync(RegisterRequest request);
        Task<ServiceResult> LoginAsync(LoginRequest request, string ipAddress);
        Task<ServiceResult> RefreshTokenAsync(string token, string ipAddress);
        Task<ServiceResult> RevokeTokenAsync(string token, string ipAddress);
    }
}
