using Microsoft.EntityFrameworkCore;
using OtoKurSkoda.Application.Defaults;
using OtoKurSkoda.Application.Dtos;
using OtoKurSkoda.Application.Services.CatalogServices.Interfaces;
using OtoKurSkoda.Domain.Entitys;
using OtoKurSkoda.Infrastructure.UnitOfWork;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace OtoKurSkoda.Application.Services.CatalogServices.Services
{
    public class AttributeDefinitionService : BaseApplicationService, IAttributeDefinitionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AttributeDefinitionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult> GetAllAsync()
        {
            var repo = _unitOfWork.GetRepository<AttributeDefinition>();

            var attributes = await repo.GetWhere(x => x.Status)
                .OrderBy(x => x.DisplayOrder)
                .ThenBy(x => x.Name)
                .ToListAsync();

            var result = attributes.Select(MapToDto).ToList();

            return SuccessListDataResult(result, "ATTRIBUTES_LISTED", "Özellikler listelendi.");
        }

        public async Task<ServiceResult> GetByIdAsync(Guid id)
        {
            var repo = _unitOfWork.GetRepository<AttributeDefinition>();

            var attribute = await repo.GetWhere(x => x.Id == id && x.Status).FirstOrDefaultAsync();

            if (attribute == null)
                return ErrorResult("ATTRIBUTE_NOT_FOUND", "Özellik bulunamadı.");

            var result = MapToDto(attribute);

            return SuccessDataResult(result, "ATTRIBUTE_FOUND", "Özellik bulundu.");
        }

        public async Task<ServiceResult> GetByCategoryIdAsync(Guid categoryId)
        {
            var repo = _unitOfWork.GetRepository<CategoryAttribute>();

            var categoryAttributes = await repo.GetWhere(x => x.CategoryId == categoryId)
                .Include(x => x.AttributeDefinition)
                .Where(x => x.AttributeDefinition.Status)
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync();

            var result = categoryAttributes.Select(ca => new AttributeDefinitionDto
            {
                Id = ca.AttributeDefinition.Id,
                Name = ca.AttributeDefinition.Name,
                Slug = ca.AttributeDefinition.Slug,
                DataType = ca.AttributeDefinition.DataType,
                Unit = ca.AttributeDefinition.Unit,
                PossibleValues = ParsePossibleValues(ca.AttributeDefinition.PossibleValues),
                DefaultValue = ca.AttributeDefinition.DefaultValue,
                IsFilterable = ca.AttributeDefinition.IsFilterable,
                IsRequired = ca.IsRequired,
                IsVisibleOnProductPage = ca.AttributeDefinition.IsVisibleOnProductPage,
                DisplayOrder = ca.DisplayOrder,
                Status = ca.AttributeDefinition.Status
            }).ToList();

            return SuccessListDataResult(result, "CATEGORY_ATTRIBUTES_LISTED", "Kategori özellikleri listelendi.");
        }

        public async Task<ServiceResult> GetFilterableAsync()
        {
            var repo = _unitOfWork.GetRepository<AttributeDefinition>();

            var attributes = await repo.GetWhere(x => x.Status && x.IsFilterable)
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync();

            var result = attributes.Select(a => new AttributeDefinitionListDto
            {
                Id = a.Id,
                Name = a.Name,
                DataType = a.DataType,
                Unit = a.Unit,
                IsFilterable = a.IsFilterable
            }).ToList();

            return SuccessListDataResult(result, "FILTERABLE_ATTRIBUTES_LISTED", "Filtrelenebilir özellikler listelendi.");
        }

        public async Task<ServiceResult> CreateAsync(CreateAttributeDefinitionRequest request)
        {
            var repo = _unitOfWork.GetRepository<AttributeDefinition>();

            var slug = GenerateSlug(request.Name);

            if (await repo.AnyAsync(x => x.Slug == slug && x.Status))
                return ErrorResult("ATTRIBUTE_EXISTS", "Bu isimde bir özellik zaten mevcut.");

            var attribute = new AttributeDefinition
            {
                Name = request.Name,
                Slug = slug,
                DataType = request.DataType,
                Unit = request.Unit,
                PossibleValues = request.PossibleValues != null ? JsonSerializer.Serialize(request.PossibleValues) : null,
                DefaultValue = request.DefaultValue,
                IsFilterable = request.IsFilterable,
                IsRequired = request.IsRequired,
                IsVisibleOnProductPage = request.IsVisibleOnProductPage,
                DisplayOrder = request.DisplayOrder
            };

            await repo.AddAsync(attribute);
            await _unitOfWork.SaveChangesAsync();

            var result = MapToDto(attribute);

            return SuccessDataResult(result, "ATTRIBUTE_CREATED", "Özellik oluşturuldu.");
        }

        public async Task<ServiceResult> UpdateAsync(UpdateAttributeDefinitionRequest request)
        {
            var repo = _unitOfWork.GetRepository<AttributeDefinition>();

            var attribute = await repo.GetByIdAsync(request.Id);

            if (attribute == null || !attribute.Status)
                return ErrorResult("ATTRIBUTE_NOT_FOUND", "Özellik bulunamadı.");

            var slug = GenerateSlug(request.Name);

            if (await repo.AnyAsync(x => x.Slug == slug && x.Id != request.Id && x.Status))
                return ErrorResult("ATTRIBUTE_EXISTS", "Bu isimde bir özellik zaten mevcut.");

            attribute.Name = request.Name;
            attribute.Slug = slug;
            attribute.DataType = request.DataType;
            attribute.Unit = request.Unit;
            attribute.PossibleValues = request.PossibleValues != null ? JsonSerializer.Serialize(request.PossibleValues) : null;
            attribute.DefaultValue = request.DefaultValue;
            attribute.IsFilterable = request.IsFilterable;
            attribute.IsRequired = request.IsRequired;
            attribute.IsVisibleOnProductPage = request.IsVisibleOnProductPage;
            attribute.DisplayOrder = request.DisplayOrder;

            repo.Update(attribute);
            await _unitOfWork.SaveChangesAsync();

            var result = MapToDto(attribute);

            return SuccessDataResult(result, "ATTRIBUTE_UPDATED", "Özellik güncellendi.");
        }

        public async Task<ServiceResult> DeleteAsync(Guid id)
        {
            var repo = _unitOfWork.GetRepository<AttributeDefinition>();
            var productAttrRepo = _unitOfWork.GetRepository<ProductAttribute>();

            var attribute = await repo.GetByIdAsync(id);

            if (attribute == null || !attribute.Status)
                return ErrorResult("ATTRIBUTE_NOT_FOUND", "Özellik bulunamadı.");

            if (await productAttrRepo.AnyAsync(x => x.AttributeDefinitionId == id))
                return ErrorResult("ATTRIBUTE_IN_USE", "Bu özellik ürünlerde kullanılıyor. Önce ürünlerden kaldırın.");

            attribute.Status = false;
            repo.Update(attribute);
            await _unitOfWork.SaveChangesAsync();

            return SuccessResult("ATTRIBUTE_DELETED", "Özellik silindi.");
        }

        public async Task<ServiceResult> AssignToCategoryAsync(Guid categoryId, Guid attributeId, bool isRequired = false)
        {
            var repo = _unitOfWork.GetRepository<CategoryAttribute>();
            var categoryRepo = _unitOfWork.GetRepository<Category>();
            var attributeRepo = _unitOfWork.GetRepository<AttributeDefinition>();

            var category = await categoryRepo.GetByIdAsync(categoryId);
            if (category == null || !category.Status)
                return ErrorResult("CATEGORY_NOT_FOUND", "Kategori bulunamadı.");

            var attribute = await attributeRepo.GetByIdAsync(attributeId);
            if (attribute == null || !attribute.Status)
                return ErrorResult("ATTRIBUTE_NOT_FOUND", "Özellik bulunamadı.");

            var existing = await repo.GetWhere(x => x.CategoryId == categoryId && x.AttributeDefinitionId == attributeId).FirstOrDefaultAsync();

            if (existing != null)
                return ErrorResult("ALREADY_ASSIGNED", "Bu özellik zaten bu kategoriye atanmış.");

            var maxOrder = await repo.GetWhere(x => x.CategoryId == categoryId).MaxAsync(x => (int?)x.DisplayOrder) ?? 0;

            var categoryAttribute = new CategoryAttribute
            {
                CategoryId = categoryId,
                AttributeDefinitionId = attributeId,
                IsRequired = isRequired,
                DisplayOrder = maxOrder + 1
            };

            await repo.AddAsync(categoryAttribute);
            await _unitOfWork.SaveChangesAsync();

            return SuccessResult("ATTRIBUTE_ASSIGNED", "Özellik kategoriye atandı.");
        }

        public async Task<ServiceResult> RemoveFromCategoryAsync(Guid categoryId, Guid attributeId)
        {
            var repo = _unitOfWork.GetRepository<CategoryAttribute>();

            var categoryAttribute = await repo.GetWhere(x => x.CategoryId == categoryId && x.AttributeDefinitionId == attributeId).FirstOrDefaultAsync();

            if (categoryAttribute == null)
                return ErrorResult("NOT_ASSIGNED", "Bu özellik bu kategoriye atanmamış.");

            repo.Delete(categoryAttribute);
            await _unitOfWork.SaveChangesAsync();

            return SuccessResult("ATTRIBUTE_REMOVED", "Özellik kategoriden kaldırıldı.");
        }

        private AttributeDefinitionDto MapToDto(AttributeDefinition a) => new()
        {
            Id = a.Id,
            Name = a.Name,
            Slug = a.Slug,
            DataType = a.DataType,
            Unit = a.Unit,
            PossibleValues = ParsePossibleValues(a.PossibleValues),
            DefaultValue = a.DefaultValue,
            IsFilterable = a.IsFilterable,
            IsRequired = a.IsRequired,
            IsVisibleOnProductPage = a.IsVisibleOnProductPage,
            DisplayOrder = a.DisplayOrder,
            Status = a.Status
        };

        private static List<string> ParsePossibleValues(string? json)
        {
            if (string.IsNullOrEmpty(json)) return new List<string>();
            try
            {
                return JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
            }
            catch
            {
                return new List<string>();
            }
        }

        private static string GenerateSlug(string text)
        {
            var slug = text.ToLowerInvariant();
            slug = slug.Replace("ı", "i").Replace("ğ", "g").Replace("ü", "u")
                       .Replace("ş", "s").Replace("ö", "o").Replace("ç", "c");
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
            slug = Regex.Replace(slug, @"\s+", "-");
            slug = Regex.Replace(slug, @"-+", "-");
            slug = slug.Trim('-');
            return slug;
        }
    }
}