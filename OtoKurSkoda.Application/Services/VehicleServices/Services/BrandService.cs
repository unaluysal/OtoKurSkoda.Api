using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OtoKurSkoda.Application.Defaults;
using OtoKurSkoda.Application.Dtos;
using OtoKurSkoda.Application.Services.VehicleServices.Interfaces;
using OtoKurSkoda.Domain.Entitys;
using OtoKurSkoda.Infrastructure.UnitOfWork;
using System.Text.RegularExpressions;

namespace OtoKurSkoda.Application.Services.VehicleServices.Services
{
    public class BrandService : BaseApplicationService, IBrandService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BrandService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResult> GetAllAsync()
        {
            var repo = _unitOfWork.GetRepository<Brand>();

            var brands = await repo.GetWhere(x => x.Status)
                .Include(x => x.VehicleModels.Where(m => m.Status))
                .OrderBy(x => x.DisplayOrder)
                .ThenBy(x => x.Name)
                .ToListAsync();

            var result = brands.Select(b => new BrandDto
            {
                Id = b.Id,
                Name = b.Name,
                Slug = b.Slug,
                LogoUrl = b.LogoUrl,
                Description = b.Description,
                DisplayOrder = b.DisplayOrder,
                Status = b.Status,
                VehicleModelCount = b.VehicleModels.Count
            }).ToList();

            return SuccessListDataResult(result, "BRANDS_LISTED", "Markalar listelendi.");
        }

        public async Task<ServiceResult> GetActiveAsync()
        {
            var repo = _unitOfWork.GetRepository<Brand>();

            var brands = await repo.GetWhere(x => x.Status)
                .OrderBy(x => x.DisplayOrder)
                .ThenBy(x => x.Name)
                .Select(x => new BrandDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Slug = x.Slug,
                    LogoUrl = x.LogoUrl,
                    DisplayOrder = x.DisplayOrder,
                    Status = x.Status
                })
                .ToListAsync();

            return SuccessListDataResult(brands, "BRANDS_LISTED", "Aktif markalar listelendi.");
        }

        public async Task<ServiceResult> GetByIdAsync(Guid id)
        {
            var repo = _unitOfWork.GetRepository<Brand>();

            var brand = await repo.GetWhere(x => x.Id == id && x.Status)
                .Include(x => x.VehicleModels.Where(m => m.Status))
                .FirstOrDefaultAsync();

            if (brand == null)
                return ErrorResult("BRAND_NOT_FOUND", "Marka bulunamadı.");

            var result = new BrandDto
            {
                Id = brand.Id,
                Name = brand.Name,
                Slug = brand.Slug,
                LogoUrl = brand.LogoUrl,
                Description = brand.Description,
                DisplayOrder = brand.DisplayOrder,
                Status = brand.Status,
                VehicleModelCount = brand.VehicleModels.Count
            };

            return SuccessDataResult(result, "BRAND_FOUND", "Marka bulundu.");
        }

        public async Task<ServiceResult> GetBySlugAsync(string slug)
        {
            var repo = _unitOfWork.GetRepository<Brand>();

            var brand = await repo.GetWhere(x => x.Slug == slug && x.Status)
                .Include(x => x.VehicleModels.Where(m => m.Status))
                .FirstOrDefaultAsync();

            if (brand == null)
                return ErrorResult("BRAND_NOT_FOUND", "Marka bulunamadı.");

            var result = new BrandDto
            {
                Id = brand.Id,
                Name = brand.Name,
                Slug = brand.Slug,
                LogoUrl = brand.LogoUrl,
                Description = brand.Description,
                DisplayOrder = brand.DisplayOrder,
                Status = brand.Status,
                VehicleModelCount = brand.VehicleModels.Count
            };

            return SuccessDataResult(result, "BRAND_FOUND", "Marka bulundu.");
        }

        public async Task<ServiceResult> CreateAsync(CreateBrandRequest request)
        {
            var repo = _unitOfWork.GetRepository<Brand>();

            var slug = GenerateSlug(request.Name);

            if (await repo.AnyAsync(x => x.Slug == slug && x.Status))
                return ErrorResult("BRAND_EXISTS", "Bu isimde bir marka zaten mevcut.");

            var brand = new Brand
            {
                Name = request.Name,
                Slug = slug,
                LogoUrl = request.LogoUrl,
                Description = request.Description,
                DisplayOrder = request.DisplayOrder
            };

            await repo.AddAsync(brand);
            await _unitOfWork.SaveChangesAsync();

            var result = _mapper.Map<BrandDto>(brand);

            return SuccessDataResult(result, "BRAND_CREATED", "Marka oluşturuldu.");
        }

        public async Task<ServiceResult> UpdateAsync(UpdateBrandRequest request)
        {
            var repo = _unitOfWork.GetRepository<Brand>();

            var brand = await repo.GetByIdAsync(request.Id);

            if (brand == null || !brand.Status)
                return ErrorResult("BRAND_NOT_FOUND", "Marka bulunamadı.");

            var slug = GenerateSlug(request.Name);

            if (await repo.AnyAsync(x => x.Slug == slug && x.Id != request.Id && x.Status))
                return ErrorResult("BRAND_EXISTS", "Bu isimde bir marka zaten mevcut.");

            brand.Name = request.Name;
            brand.Slug = slug;
            brand.LogoUrl = request.LogoUrl;
            brand.Description = request.Description;
            brand.DisplayOrder = request.DisplayOrder;

            repo.Update(brand);
            await _unitOfWork.SaveChangesAsync();

            var result = _mapper.Map<BrandDto>(brand);

            return SuccessDataResult(result, "BRAND_UPDATED", "Marka güncellendi.");
        }

        public async Task<ServiceResult> DeleteAsync(Guid id)
        {
            var repo = _unitOfWork.GetRepository<Brand>();

            var brand = await repo.GetWhere(x => x.Id == id && x.Status)
                .Include(x => x.VehicleModels.Where(m => m.Status))
                .FirstOrDefaultAsync();

            if (brand == null)
                return ErrorResult("BRAND_NOT_FOUND", "Marka bulunamadı.");

            if (brand.VehicleModels.Any())
                return ErrorResult("BRAND_HAS_MODELS", "Bu markaya ait araç modelleri var. Önce modelleri silin.");

            brand.Status = false;
            repo.Update(brand);
            await _unitOfWork.SaveChangesAsync();

            return SuccessResult("BRAND_DELETED", "Marka silindi.");
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
