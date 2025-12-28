using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OtoKurSkoda.Application.Defaults;
using OtoKurSkoda.Application.Dtos;
using OtoKurSkoda.Application.Services.CatalogServices.Interfaces;
using OtoKurSkoda.Domain.Entitys;
using OtoKurSkoda.Infrastructure.UnitOfWork;
using System.Text.RegularExpressions;

namespace OtoKurSkoda.Application.Services.CatalogServices.Services
{
    public class ManufacturerService : BaseApplicationService, IManufacturerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ManufacturerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResult> GetAllAsync()
        {
            var repo = _unitOfWork.GetRepository<Manufacturer>();

            var manufacturers = await repo.GetWhere(x => x.Status)
                .Include(x => x.Products.Where(p => p.Status))
                .OrderBy(x => x.DisplayOrder)
                .ThenBy(x => x.Name)
                .ToListAsync();

            var result = manufacturers.Select(m => new ManufacturerDto
            {
                Id = m.Id,
                Name = m.Name,
                Slug = m.Slug,
                LogoUrl = m.LogoUrl,
                Website = m.Website,
                Description = m.Description,
                DisplayOrder = m.DisplayOrder,
                Status = m.Status,
                ProductCount = m.Products.Count
            }).ToList();

            return SuccessListDataResult(result, "MANUFACTURERS_LISTED", "Üreticiler listelendi.");
        }

        public async Task<ServiceResult> GetActiveAsync()
        {
            var repo = _unitOfWork.GetRepository<Manufacturer>();

            var manufacturers = await repo.GetWhere(x => x.Status)
                .OrderBy(x => x.DisplayOrder)
                .ThenBy(x => x.Name)
                .Select(x => new ManufacturerListDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Slug = x.Slug,
                    LogoUrl = x.LogoUrl
                })
                .ToListAsync();

            return SuccessListDataResult(manufacturers, "MANUFACTURERS_LISTED", "Aktif üreticiler listelendi.");
        }

        public async Task<ServiceResult> GetByIdAsync(Guid id)
        {
            var repo = _unitOfWork.GetRepository<Manufacturer>();

            var manufacturer = await repo.GetWhere(x => x.Id == id && x.Status)
                .Include(x => x.Products.Where(p => p.Status))
                .FirstOrDefaultAsync();

            if (manufacturer == null)
                return ErrorResult("MANUFACTURER_NOT_FOUND", "Üretici bulunamadı.");

            var result = new ManufacturerDto
            {
                Id = manufacturer.Id,
                Name = manufacturer.Name,
                Slug = manufacturer.Slug,
                LogoUrl = manufacturer.LogoUrl,
                Website = manufacturer.Website,
                Description = manufacturer.Description,
                DisplayOrder = manufacturer.DisplayOrder,
                Status = manufacturer.Status,
                ProductCount = manufacturer.Products.Count
            };

            return SuccessDataResult(result, "MANUFACTURER_FOUND", "Üretici bulundu.");
        }

        public async Task<ServiceResult> GetBySlugAsync(string slug)
        {
            var repo = _unitOfWork.GetRepository<Manufacturer>();

            var manufacturer = await repo.GetWhere(x => x.Slug == slug && x.Status)
                .Include(x => x.Products.Where(p => p.Status))
                .FirstOrDefaultAsync();

            if (manufacturer == null)
                return ErrorResult("MANUFACTURER_NOT_FOUND", "Üretici bulunamadı.");

            var result = new ManufacturerDto
            {
                Id = manufacturer.Id,
                Name = manufacturer.Name,
                Slug = manufacturer.Slug,
                LogoUrl = manufacturer.LogoUrl,
                Website = manufacturer.Website,
                Description = manufacturer.Description,
                DisplayOrder = manufacturer.DisplayOrder,
                Status = manufacturer.Status,
                ProductCount = manufacturer.Products.Count
            };

            return SuccessDataResult(result, "MANUFACTURER_FOUND", "Üretici bulundu.");
        }

        public async Task<ServiceResult> CreateAsync(CreateManufacturerRequest request)
        {
            var repo = _unitOfWork.GetRepository<Manufacturer>();

            var slug = GenerateSlug(request.Name);

            if (await repo.AnyAsync(x => x.Slug == slug && x.Status))
                return ErrorResult("MANUFACTURER_EXISTS", "Bu isimde bir üretici zaten mevcut.");

            var manufacturer = new Manufacturer
            {
                Name = request.Name,
                Slug = slug,
                LogoUrl = request.LogoUrl,
                Website = request.Website,
                Description = request.Description,
                DisplayOrder = request.DisplayOrder
            };

            await repo.AddAsync(manufacturer);
            await _unitOfWork.SaveChangesAsync();

            return SuccessDataResult(new ManufacturerDto
            {
                Id = manufacturer.Id,
                Name = manufacturer.Name,
                Slug = manufacturer.Slug,
                LogoUrl = manufacturer.LogoUrl,
                Website = manufacturer.Website,
                Description = manufacturer.Description,
                DisplayOrder = manufacturer.DisplayOrder,
                Status = manufacturer.Status
            }, "MANUFACTURER_CREATED", "Üretici oluşturuldu.");
        }

        public async Task<ServiceResult> UpdateAsync(UpdateManufacturerRequest request)
        {
            var repo = _unitOfWork.GetRepository<Manufacturer>();

            var manufacturer = await repo.GetByIdAsync(request.Id);

            if (manufacturer == null || !manufacturer.Status)
                return ErrorResult("MANUFACTURER_NOT_FOUND", "Üretici bulunamadı.");

            var slug = GenerateSlug(request.Name);

            if (await repo.AnyAsync(x => x.Slug == slug && x.Id != request.Id && x.Status))
                return ErrorResult("MANUFACTURER_EXISTS", "Bu isimde bir üretici zaten mevcut.");

            manufacturer.Name = request.Name;
            manufacturer.Slug = slug;
            manufacturer.LogoUrl = request.LogoUrl;
            manufacturer.Website = request.Website;
            manufacturer.Description = request.Description;
            manufacturer.DisplayOrder = request.DisplayOrder;

            repo.Update(manufacturer);
            await _unitOfWork.SaveChangesAsync();

            return SuccessDataResult(new ManufacturerDto
            {
                Id = manufacturer.Id,
                Name = manufacturer.Name,
                Slug = manufacturer.Slug,
                LogoUrl = manufacturer.LogoUrl,
                Website = manufacturer.Website,
                Description = manufacturer.Description,
                DisplayOrder = manufacturer.DisplayOrder,
                Status = manufacturer.Status
            }, "MANUFACTURER_UPDATED", "Üretici güncellendi.");
        }

        public async Task<ServiceResult> DeleteAsync(Guid id)
        {
            var repo = _unitOfWork.GetRepository<Manufacturer>();

            var manufacturer = await repo.GetWhere(x => x.Id == id && x.Status)
                .Include(x => x.Products.Where(p => p.Status))
                .FirstOrDefaultAsync();

            if (manufacturer == null)
                return ErrorResult("MANUFACTURER_NOT_FOUND", "Üretici bulunamadı.");

            if (manufacturer.Products.Any())
                return ErrorResult("MANUFACTURER_HAS_PRODUCTS", "Bu üreticiye ait ürünler var. Önce ürünlerin üreticisini değiştirin.");

            manufacturer.Status = false;
            repo.Update(manufacturer);
            await _unitOfWork.SaveChangesAsync();

            return SuccessResult("MANUFACTURER_DELETED", "Üretici silindi.");
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
