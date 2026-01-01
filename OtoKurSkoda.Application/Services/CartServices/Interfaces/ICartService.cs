using OtoKurSkoda.Application.Defaults;

namespace OtoKurSkoda.Application.Services.CartServices.Interfaces
{
    public interface ICartService
    {
        Task<ServiceResult> GetCartAsync(Guid userId);
        Task<ServiceResult> AddToCartAsync(Guid userId, Guid productId, int quantity);
        Task<ServiceResult> UpdateQuantityAsync(Guid userId, Guid cartItemId, int quantity);
        Task<ServiceResult> RemoveFromCartAsync(Guid userId, Guid cartItemId);
        Task<ServiceResult> ClearCartAsync(Guid userId);
        Task<ServiceResult> GetCartItemCountAsync(Guid userId);
    }
}
