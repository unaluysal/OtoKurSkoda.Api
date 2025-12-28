using OtoKurSkoda.Application.Defaults;
using OtoKurSkoda.Application.Dtos;

namespace OtoKurSkoda.Application.Services.ProductServices.Interfaces
{
    public interface IProductImageService
    {
        Task<ServiceResult> GetByProductIdAsync(Guid productId);
        Task<ServiceResult> AddImageAsync(AddProductImageRequest request);
        Task<ServiceResult> UpdateImageAsync(UpdateProductImageRequest request);
        Task<ServiceResult> DeleteImageAsync(Guid id);
        Task<ServiceResult> SetMainImageAsync(Guid productId, Guid imageId);
        Task<ServiceResult> ReorderImagesAsync(Guid productId, List<Guid> imageIds);
    }
}
