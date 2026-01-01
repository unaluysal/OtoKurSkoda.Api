using OtoKurSkoda.Application.Defaults;
using OtoKurSkoda.Application.Dtos;

namespace OtoKurSkoda.Application.Services.OrderServices.Interfaces
{
    public interface IOrderService
    {
        // Müşteri İşlemleri
        Task<ServiceResult> CreateOrderFromCartAsync(Guid userId, CreateOrderRequest request);
        Task<ServiceResult> GetMyOrdersAsync(Guid userId, int pageNumber = 1, int pageSize = 20);
        Task<ServiceResult> GetMyOrderDetailAsync(Guid userId, Guid orderId);
        Task<ServiceResult> CancelOrderAsync(Guid userId, Guid orderId);

        // Admin İşlemleri
        Task<ServiceResult> GetAllOrdersAsync(OrderFilterRequest filter);
        Task<ServiceResult> GetOrderByIdAsync(Guid orderId);
        Task<ServiceResult> GetOrderByNumberAsync(string orderNumber);
        Task<ServiceResult> UpdateOrderStatusAsync(UpdateOrderStatusRequest request);
        Task<ServiceResult> UpdateShippingInfoAsync(UpdateShippingInfoRequest request);
        Task<ServiceResult> GetOrderStatisticsAsync();
    }
}
