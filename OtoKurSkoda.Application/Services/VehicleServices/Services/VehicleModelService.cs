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
    public class VehicleModelService : BaseApplicationService, IVehicleModelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VehicleModelService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResult> GetAllAsync()
        {
            var repo = _unitOfWork.GetRepository<VehicleModel>();

            var models = await repo.GetWhere(x => x.Status)
                .Include(x => x.Brand)
                .Include(x => x.Generations.Where(g => g.Status))
                .OrderBy(x => x.Brand.Name)
                .ThenBy(x => x.DisplayOrder)
                .ThenBy(x => x.Name)
                .ToListAsync();

            var result = models.Select(m => new VehicleModelDto
            {
                Id = m.Id,
                BrandId = m.BrandId,
                BrandName = m.Brand.Name,
                Name = m.Name,
                Slug = m.Slug,
                ImageUrl = m.ImageUrl,
                Description = m.Description,
                DisplayOrder = m.DisplayOrder,
                Status = m.Status,
                GenerationCount = m.Generations.Count
            }).ToList();

            return SuccessListDataResult(result, "VEHICLE_MODELS_LISTED", "Araç modelleri listelendi.");
        }

        public async Task<ServiceResult> GetByBrandIdAsync(Guid brandId)
        {
            var repo = _unitOfWork.GetRepository<VehicleModel>();

            var models = await repo.GetWhere(x => x.BrandId == brandId && x.Status)
                .Include(x => x.Brand)
                .Include(x => x.Generations.Where(g => g.Status))
                .OrderBy(x => x.DisplayOrder)
                .ThenBy(x => x.Name)
                .ToListAsync();

            var result = models.Select(m => new VehicleModelDto
            {
                Id = m.Id,
                BrandId = m.BrandId,
                BrandName = m.Brand.Name,
                Name = m.Name,
                Slug = m.Slug,
                ImageUrl = m.ImageUrl,
                Description = m.Description,
                DisplayOrder = m.DisplayOrder,
                Status = m.Status,
                GenerationCount = m.Generations.Count
            }).ToList();

            return SuccessListDataResult(result, "VEHICLE_MODELS_LISTED", "Araç modelleri listelendi.");
        }

        public async Task<ServiceResult> GetByIdAsync(Guid id)
        {
            var repo = _unitOfWork.GetRepository<VehicleModel>();

            var model = await repo.GetWhere(x => x.Id == id && x.Status)
                .Include(x => x.Brand)
                .Include(x => x.Generations.Where(g => g.Status))
                .FirstOrDefaultAsync();

            if (model == null)
                return ErrorResult("VEHICLE_MODEL_NOT_FOUND", "Araç modeli bulunamadı.");

            var result = new VehicleModelDto
            {
                Id = model.Id,
                BrandId = model.BrandId,
                BrandName = model.Brand.Name,
                Name = model.Name,
                Slug = model.Slug,
                ImageUrl = model.ImageUrl,
                Description = model.Description,
                DisplayOrder = model.DisplayOrder,
                Status = model.Status,
                GenerationCount = model.Generations.Count
            };

            return SuccessDataResult(result, "VEHICLE_MODEL_FOUND", "Araç modeli bulundu.");
        }

        public async Task<ServiceResult> GetBySlugAsync(string brandSlug, string modelSlug)
        {
            var repo = _unitOfWork.GetRepository<VehicleModel>();

            var model = await repo.GetWhere(x => x.Brand.Slug == brandSlug && x.Slug == modelSlug && x.Status)
                .Include(x => x.Brand)
                .Include(x => x.Generations.Where(g => g.Status))
                .FirstOrDefaultAsync();

            if (model == null)
                return ErrorResult("VEHICLE_MODEL_NOT_FOUND", "Araç modeli bulunamadı.");

            var result = new VehicleModelDto
            {
                Id = model.Id,
                BrandId = model.BrandId,
                BrandName = model.Brand.Name,
                Name = model.Name,
                Slug = model.Slug,
                ImageUrl = model.ImageUrl,
                Description = model.Description,
                DisplayOrder = model.DisplayOrder,
                Status = model.Status,
                GenerationCount = model.Generations.Count
            };

            return SuccessDataResult(result, "VEHICLE_MODEL_FOUND", "Araç modeli bulundu.");
        }

        public async Task<ServiceResult> CreateAsync(CreateVehicleModelRequest request)
        {
            var repo = _unitOfWork.GetRepository<VehicleModel>();
            var brandRepo = _unitOfWork.GetRepository<Brand>();

            var brand = await brandRepo.GetByIdAsync(request.BrandId);
            if (brand == null || !brand.Status)
                return ErrorResult("BRAND_NOT_FOUND", "Marka bulunamadı.");

            var slug = GenerateSlug(request.Name);

            if (await repo.AnyAsync(x => x.BrandId == request.BrandId && x.Slug == slug && x.Status))
                return ErrorResult("VEHICLE_MODEL_EXISTS", "Bu marka altında aynı isimde bir model zaten mevcut.");

            var model = new VehicleModel
            {
                BrandId = request.BrandId,
                Name = request.Name,
                Slug = slug,
                ImageUrl = request.ImageUrl,
                Description = request.Description,
                DisplayOrder = request.DisplayOrder
            };

            await repo.AddAsync(model);
            await _unitOfWork.SaveChangesAsync();

            return SuccessDataResult(new VehicleModelDto
            {
                Id = model.Id,
                BrandId = model.BrandId,
                BrandName = brand.Name,
                Name = model.Name,
                Slug = model.Slug,
                ImageUrl = model.ImageUrl,
                Description = model.Description,
                DisplayOrder = model.DisplayOrder,
                Status = model.Status
            }, "VEHICLE_MODEL_CREATED", "Araç modeli oluşturuldu.");
        }

        public async Task<ServiceResult> UpdateAsync(UpdateVehicleModelRequest request)
        {
            var repo = _unitOfWork.GetRepository<VehicleModel>();
            var brandRepo = _unitOfWork.GetRepository<Brand>();

            var model = await repo.GetWhere(x => x.Id == request.Id && x.Status)
                .Include(x => x.Brand)
                .FirstOrDefaultAsync();

            if (model == null)
                return ErrorResult("VEHICLE_MODEL_NOT_FOUND", "Araç modeli bulunamadı.");

            var brand = await brandRepo.GetByIdAsync(request.BrandId);
            if (brand == null || !brand.Status)
                return ErrorResult("BRAND_NOT_FOUND", "Marka bulunamadı.");

            var slug = GenerateSlug(request.Name);

            if (await repo.AnyAsync(x => x.BrandId == request.BrandId && x.Slug == slug && x.Id != request.Id && x.Status))
                return ErrorResult("VEHICLE_MODEL_EXISTS", "Bu marka altında aynı isimde bir model zaten mevcut.");

            model.BrandId = request.BrandId;
            model.Name = request.Name;
            model.Slug = slug;
            model.ImageUrl = request.ImageUrl;
            model.Description = request.Description;
            model.DisplayOrder = request.DisplayOrder;

            repo.Update(model);
            await _unitOfWork.SaveChangesAsync();

            return SuccessDataResult(new VehicleModelDto
            {
                Id = model.Id,
                BrandId = model.BrandId,
                BrandName = brand.Name,
                Name = model.Name,
                Slug = model.Slug,
                ImageUrl = model.ImageUrl,
                Description = model.Description,
                DisplayOrder = model.DisplayOrder,
                Status = model.Status
            }, "VEHICLE_MODEL_UPDATED", "Araç modeli güncellendi.");
        }

        public async Task<ServiceResult> DeleteAsync(Guid id)
        {
            var repo = _unitOfWork.GetRepository<VehicleModel>();

            var model = await repo.GetWhere(x => x.Id == id && x.Status)
                .Include(x => x.Generations.Where(g => g.Status))
                .FirstOrDefaultAsync();

            if (model == null)
                return ErrorResult("VEHICLE_MODEL_NOT_FOUND", "Araç modeli bulunamadı.");

            if (model.Generations.Any())
                return ErrorResult("VEHICLE_MODEL_HAS_GENERATIONS", "Bu modele ait nesiller var. Önce nesilleri silin.");

            model.Status = false;
            repo.Update(model);
            await _unitOfWork.SaveChangesAsync();

            return SuccessResult("VEHICLE_MODEL_DELETED", "Araç modeli silindi.");
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
