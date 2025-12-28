using OtoKurSkoda.Application.Defaults;
using OtoKurSkoda.Application.Dtos;

namespace OtoKurSkoda.Application.Services.ProductServices.Interfaces
{
    public interface IProductAttributeService
    {
        Task<ServiceResult> GetByProductIdAsync(Guid productId);
        Task<ServiceResult> SetAttributesAsync(SetProductAttributesRequest request);
        Task<ServiceResult> UpdateAttributeAsync(UpdateProductAttributeRequest request);
        Task<ServiceResult> DeleteAttributeAsync(Guid id);
    }
}
