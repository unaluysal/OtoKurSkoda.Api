using Microsoft.EntityFrameworkCore;
using OtoKurSkoda.Application.Defaults;
using OtoKurSkoda.Application.Dtos;
using OtoKurSkoda.Application.Services.ProductServices.Interfaces;
using OtoKurSkoda.Domain.Entitys;
using OtoKurSkoda.Infrastructure.UnitOfWork;

namespace OtoKurSkoda.Application.Services.ProductServices.Services
{
    public class ProductAttributeService : BaseApplicationService, IProductAttributeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductAttributeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult> GetByProductIdAsync(Guid productId)
        {
            var repo = _unitOfWork.GetRepository<ProductAttribute>();

            var attributes = await repo.GetWhere(x => x.ProductId == productId && x.Status)
                .Include(x => x.AttributeDefinition)
                .OrderBy(x => x.AttributeDefinition.DisplayOrder)
                .ToListAsync();

            var result = attributes.Select(a => new ProductAttributeDto
            {
                Id = a.Id,
                ProductId = a.ProductId,
                AttributeDefinitionId = a.AttributeDefinitionId,
                AttributeName = a.AttributeDefinition.Name,
                Unit = a.AttributeDefinition.Unit,
                Value = a.Value
            }).ToList();

            return SuccessListDataResult(result, "PRODUCT_ATTRIBUTES_LISTED", "Ürün özellikleri listelendi.");
        }

        public async Task<ServiceResult> SetAttributesAsync(SetProductAttributesRequest request)
        {
            var repo = _unitOfWork.GetRepository<ProductAttribute>();
            var productRepo = _unitOfWork.GetRepository<Product>();
            var attrDefRepo = _unitOfWork.GetRepository<AttributeDefinition>();

            var product = await productRepo.GetByIdAsync(request.ProductId);
            if (product == null || !product.Status)
                return ErrorResult("PRODUCT_NOT_FOUND", "Ürün bulunamadı.");

            // Mevcut özellikleri soft delete
            var existingAttributes = await repo.GetWhere(x => x.ProductId == request.ProductId && x.Status).ToListAsync();
            foreach (var attr in existingAttributes)
            {
                attr.Status = false;
                repo.Update(attr);
            }

            // Yeni özellikleri ekle
            foreach (var attrRequest in request.Attributes)
            {
                var attrDef = await attrDefRepo.GetByIdAsync(attrRequest.AttributeDefinitionId);
                if (attrDef == null || !attrDef.Status) continue;

                var productAttribute = new ProductAttribute
                {
                    ProductId = request.ProductId,
                    AttributeDefinitionId = attrRequest.AttributeDefinitionId,
                    Value = attrRequest.Value
                };
                await repo.AddAsync(productAttribute);
            }

            await _unitOfWork.SaveChangesAsync();

            return SuccessResult("PRODUCT_ATTRIBUTES_SET", $"{request.Attributes.Count} özellik ayarlandı.");
        }

        public async Task<ServiceResult> UpdateAttributeAsync(UpdateProductAttributeRequest request)
        {
            var repo = _unitOfWork.GetRepository<ProductAttribute>();

            var attribute = await repo.GetByIdAsync(request.Id);
            if (attribute == null || !attribute.Status)
                return ErrorResult("ATTRIBUTE_NOT_FOUND", "Özellik bulunamadı.");

            attribute.Value = request.Value;
            repo.Update(attribute);
            await _unitOfWork.SaveChangesAsync();

            return SuccessResult("PRODUCT_ATTRIBUTE_UPDATED", "Ürün özelliği güncellendi.");
        }

        public async Task<ServiceResult> DeleteAttributeAsync(Guid id)
        {
            var repo = _unitOfWork.GetRepository<ProductAttribute>();

            var attribute = await repo.GetByIdAsync(id);
            if (attribute == null || !attribute.Status)
                return ErrorResult("ATTRIBUTE_NOT_FOUND", "Özellik bulunamadı.");

            attribute.Status = false;
            repo.Update(attribute);
            await _unitOfWork.SaveChangesAsync();

            return SuccessResult("PRODUCT_ATTRIBUTE_DELETED", "Ürün özelliği silindi.");
        }
    }
}