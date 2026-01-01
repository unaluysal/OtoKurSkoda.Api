using Microsoft.EntityFrameworkCore;
using OtoKurSkoda.Application.Defaults;
using OtoKurSkoda.Application.Dtos;
using OtoKurSkoda.Application.Services.ProductServices.Interfaces;
using OtoKurSkoda.Domain.Entitys;
using OtoKurSkoda.Infrastructure.UnitOfWork;

namespace OtoKurSkoda.Application.Services.ProductServices.Services
{
    public class ProductCompatibilityService : BaseApplicationService, IProductCompatibilityService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductCompatibilityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult> GetByProductIdAsync(Guid productId)
        {
            var repo = _unitOfWork.GetRepository<ProductCompatibility>();

            var compatibilities = await repo.GetWhere(x => x.ProductId == productId && x.Status)
                .Include(x => x.VehicleGeneration)
                    .ThenInclude(g => g.VehicleModel)
                        .ThenInclude(m => m.Brand)
                .OrderBy(x => x.VehicleGeneration.VehicleModel.Brand.Name)
                .ThenBy(x => x.VehicleGeneration.VehicleModel.Name)
                .ThenBy(x => x.VehicleGeneration.StartYear)
                .ToListAsync();

            var result = compatibilities.Select(c => new ProductCompatibilityDto
            {
                Id = c.Id,
                ProductId = c.ProductId,
                VehicleGenerationId = c.VehicleGenerationId,
                BrandName = c.VehicleGeneration.VehicleModel.Brand.Name,
                VehicleModelName = c.VehicleGeneration.VehicleModel.Name,
                VehicleGenerationName = c.VehicleGeneration.Name,
                Code = c.VehicleGeneration.Code,
                StartYear = c.VehicleGeneration.StartYear,
                EndYear = c.VehicleGeneration.EndYear,
                Notes = c.Notes
            }).ToList();

            return SuccessListDataResult(result, "COMPATIBILITIES_LISTED", "Araç uyumlulukları listelendi.");
        }

        public async Task<ServiceResult> GetProductsByVehicleAsync(Guid vehicleGenerationId)
        {
            var repo = _unitOfWork.GetRepository<ProductCompatibility>();

            var productIds = await repo.GetWhere(x => x.VehicleGenerationId == vehicleGenerationId && x.Status)
                .Select(x => x.ProductId)
                .Distinct()
                .ToListAsync();

            return SuccessDataResult(productIds, "PRODUCT_IDS_LISTED", "Uyumlu ürün ID'leri listelendi.");
        }

        public async Task<ServiceResult> AddCompatibilityAsync(AddProductCompatibilityRequest request)
        {
            var repo = _unitOfWork.GetRepository<ProductCompatibility>();
            var productRepo = _unitOfWork.GetRepository<Product>();
            var generationRepo = _unitOfWork.GetRepository<VehicleGeneration>();

            var product = await productRepo.GetByIdAsync(request.ProductId);
            if (product == null || !product.Status)
                return ErrorResult("PRODUCT_NOT_FOUND", "Ürün bulunamadı.");

            var generation = await generationRepo.GetWhere(x => x.Id == request.VehicleGenerationId && x.Status)
                .Include(x => x.VehicleModel)
                    .ThenInclude(m => m.Brand)
                .FirstOrDefaultAsync();

            if (generation == null)
                return ErrorResult("VEHICLE_GENERATION_NOT_FOUND", "Araç nesli bulunamadı.");

            if (await repo.AnyAsync(x => x.ProductId == request.ProductId && x.VehicleGenerationId == request.VehicleGenerationId && x.Status))
                return ErrorResult("COMPATIBILITY_EXISTS", "Bu uyumluluk zaten mevcut.");

            var compatibility = new ProductCompatibility
            {
                ProductId = request.ProductId,
                VehicleGenerationId = request.VehicleGenerationId,
                Notes = request.Notes,
                Status = true
            };

            await repo.AddAsync(compatibility);
            await _unitOfWork.SaveChangesAsync();

            return SuccessDataResult(new ProductCompatibilityDto
            {
                Id = compatibility.Id,
                ProductId = compatibility.ProductId,
                VehicleGenerationId = compatibility.VehicleGenerationId,
                BrandName = generation.VehicleModel.Brand.Name,
                VehicleModelName = generation.VehicleModel.Name,
                VehicleGenerationName = generation.Name,
                Code = generation.Code,
                StartYear = generation.StartYear,
                EndYear = generation.EndYear,
                Notes = compatibility.Notes
            }, "COMPATIBILITY_ADDED", "Araç uyumluluğu eklendi.");
        }

        public async Task<ServiceResult> RemoveCompatibilityAsync(Guid id)
        {
            var repo = _unitOfWork.GetRepository<ProductCompatibility>();

            var compatibility = await repo.GetByIdAsync(id);
            if (compatibility == null || !compatibility.Status)
                return ErrorResult("COMPATIBILITY_NOT_FOUND", "Uyumluluk bulunamadı.");

            compatibility.Status = false;
            repo.Update(compatibility);
            await _unitOfWork.SaveChangesAsync();

            return SuccessResult("COMPATIBILITY_REMOVED", "Araç uyumluluğu kaldırıldı.");
        }

        public async Task<ServiceResult> SetCompatibilitiesAsync(SetProductCompatibilitiesRequest request)
        {
            var repo = _unitOfWork.GetRepository<ProductCompatibility>();
            var productRepo = _unitOfWork.GetRepository<Product>();

            var product = await productRepo.GetByIdAsync(request.ProductId);
            if (product == null || !product.Status)
                return ErrorResult("PRODUCT_NOT_FOUND", "Ürün bulunamadı.");

            // Mevcut TÜM uyumlulukları getir (aktif ve pasif)
            var existingCompatibilities = await repo.GetWhere(x => x.ProductId == request.ProductId).ToListAsync();

            // İstenen generation ID'leri set olarak
            var requestedIds = request.VehicleGenerationIds.ToHashSet();

            // Mevcut kayıtları işle
            foreach (var comp in existingCompatibilities)
            {
                if (requestedIds.Contains(comp.VehicleGenerationId))
                {
                    // Bu generation isteniyor - aktif yap
                    comp.Status = true;
                    repo.Update(comp);
                    requestedIds.Remove(comp.VehicleGenerationId); // İşlendi, listeden çıkar
                }
                else
                {
                    // Bu generation istenmiyor - pasif yap
                    if (comp.Status)
                    {
                        comp.Status = false;
                        repo.Update(comp);
                    }
                }
            }

            // Kalan ID'ler için yeni kayıt oluştur (daha önce hiç eklenmemiş olanlar)
            foreach (var generationId in requestedIds)
            {
                var compatibility = new ProductCompatibility
                {
                    ProductId = request.ProductId,
                    VehicleGenerationId = generationId,
                    Status = true
                };
                await repo.AddAsync(compatibility);
            }

            await _unitOfWork.SaveChangesAsync();

            return SuccessResult("COMPATIBILITIES_SET", $"{request.VehicleGenerationIds.Count} araç uyumluluğu ayarlandı.");
        }
    }
}