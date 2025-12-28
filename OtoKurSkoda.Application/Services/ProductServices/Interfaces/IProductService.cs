using OtoKurSkoda.Application.Defaults;
using OtoKurSkoda.Application.Dtos;

namespace OtoKurSkoda.Application.Services.ProductServices.Interfaces
{
    public interface IProductService
    {
        Task<ServiceResult> GetAllAsync(ProductFilterRequest filter);
        Task<ServiceResult> GetByIdAsync(Guid id);
        Task<ServiceResult> GetBySlugAsync(string slug);
        Task<ServiceResult> GetByCategoryAsync(Guid categoryId, int pageNumber = 1, int pageSize = 20);
        Task<ServiceResult> GetByManufacturerAsync(Guid manufacturerId, int pageNumber = 1, int pageSize = 20);
        Task<ServiceResult> GetByVehicleAsync(Guid vehicleGenerationId, int pageNumber = 1, int pageSize = 20);
        Task<ServiceResult> GetFeaturedAsync(int count = 10);
        Task<ServiceResult> GetNewArrivalsAsync(int count = 10);
        Task<ServiceResult> GetBestSellersAsync(int count = 10);
        Task<ServiceResult> GetOnSaleAsync(int count = 10);
        Task<ServiceResult> SearchAsync(string searchTerm, int pageNumber = 1, int pageSize = 20);
        Task<ServiceResult> CreateAsync(CreateProductRequest request);
        Task<ServiceResult> UpdateAsync(UpdateProductRequest request);
        Task<ServiceResult> DeleteAsync(Guid id);
        Task<ServiceResult> UpdateStockAsync(Guid id, int quantity);
    }
}
