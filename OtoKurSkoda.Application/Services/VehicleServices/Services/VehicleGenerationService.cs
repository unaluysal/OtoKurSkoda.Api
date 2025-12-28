using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OtoKurSkoda.Application.Defaults;
using OtoKurSkoda.Application.Dtos;
using OtoKurSkoda.Application.Services.VehicleServices.Interfaces;
using OtoKurSkoda.Domain.Entitys;
using OtoKurSkoda.Infrastructure.UnitOfWork;

namespace OtoKurSkoda.Application.Services.VehicleServices.Services
{
    public class VehicleGenerationService : BaseApplicationService, IVehicleGenerationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VehicleGenerationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResult> GetAllAsync()
        {
            var repo = _unitOfWork.GetRepository<VehicleGeneration>();

            var generations = await repo.GetWhere(x => x.Status)
                .Include(x => x.VehicleModel)
                    .ThenInclude(x => x.Brand)
                .OrderBy(x => x.VehicleModel.Brand.Name)
                .ThenBy(x => x.VehicleModel.Name)
                .ThenBy(x => x.StartYear)
                .ToListAsync();

            var result = generations.Select(g => new VehicleGenerationDto
            {
                Id = g.Id,
                VehicleModelId = g.VehicleModelId,
                VehicleModelName = g.VehicleModel.Name,
                BrandName = g.VehicleModel.Brand.Name,
                Name = g.Name,
                Code = g.Code,
                StartYear = g.StartYear,
                EndYear = g.EndYear,
                ImageUrl = g.ImageUrl,
                Description = g.Description,
                Status = g.Status
            }).ToList();

            return SuccessListDataResult(result, "VEHICLE_GENERATIONS_LISTED", "Araç nesilleri listelendi.");
        }

        public async Task<ServiceResult> GetByModelIdAsync(Guid modelId)
        {
            var repo = _unitOfWork.GetRepository<VehicleGeneration>();

            var generations = await repo.GetWhere(x => x.VehicleModelId == modelId && x.Status)
                .Include(x => x.VehicleModel)
                    .ThenInclude(x => x.Brand)
                .OrderBy(x => x.StartYear)
                .ToListAsync();

            var result = generations.Select(g => new VehicleGenerationDto
            {
                Id = g.Id,
                VehicleModelId = g.VehicleModelId,
                VehicleModelName = g.VehicleModel.Name,
                BrandName = g.VehicleModel.Brand.Name,
                Name = g.Name,
                Code = g.Code,
                StartYear = g.StartYear,
                EndYear = g.EndYear,
                ImageUrl = g.ImageUrl,
                Description = g.Description,
                Status = g.Status
            }).ToList();

            return SuccessListDataResult(result, "VEHICLE_GENERATIONS_LISTED", "Araç nesilleri listelendi.");
        }

        public async Task<ServiceResult> GetByIdAsync(Guid id)
        {
            var repo = _unitOfWork.GetRepository<VehicleGeneration>();

            var generation = await repo.GetWhere(x => x.Id == id && x.Status)
                .Include(x => x.VehicleModel)
                    .ThenInclude(x => x.Brand)
                .FirstOrDefaultAsync();

            if (generation == null)
                return ErrorResult("VEHICLE_GENERATION_NOT_FOUND", "Araç nesli bulunamadı.");

            var result = new VehicleGenerationDto
            {
                Id = generation.Id,
                VehicleModelId = generation.VehicleModelId,
                VehicleModelName = generation.VehicleModel.Name,
                BrandName = generation.VehicleModel.Brand.Name,
                Name = generation.Name,
                Code = generation.Code,
                StartYear = generation.StartYear,
                EndYear = generation.EndYear,
                ImageUrl = generation.ImageUrl,
                Description = generation.Description,
                Status = generation.Status
            };

            return SuccessDataResult(result, "VEHICLE_GENERATION_FOUND", "Araç nesli bulundu.");
        }

        public async Task<ServiceResult> CreateAsync(CreateVehicleGenerationRequest request)
        {
            var repo = _unitOfWork.GetRepository<VehicleGeneration>();
            var modelRepo = _unitOfWork.GetRepository<VehicleModel>();

            var vehicleModel = await modelRepo.GetWhere(x => x.Id == request.VehicleModelId && x.Status)
                .Include(x => x.Brand)
                .FirstOrDefaultAsync();

            if (vehicleModel == null)
                return ErrorResult("VEHICLE_MODEL_NOT_FOUND", "Araç modeli bulunamadı.");

            var generation = new VehicleGeneration
            {
                VehicleModelId = request.VehicleModelId,
                Name = request.Name,
                Code = request.Code,
                StartYear = request.StartYear,
                EndYear = request.EndYear,
                ImageUrl = request.ImageUrl,
                Description = request.Description
            };

            await repo.AddAsync(generation);
            await _unitOfWork.SaveChangesAsync();

            return SuccessDataResult(new VehicleGenerationDto
            {
                Id = generation.Id,
                VehicleModelId = generation.VehicleModelId,
                VehicleModelName = vehicleModel.Name,
                BrandName = vehicleModel.Brand.Name,
                Name = generation.Name,
                Code = generation.Code,
                StartYear = generation.StartYear,
                EndYear = generation.EndYear,
                ImageUrl = generation.ImageUrl,
                Description = generation.Description,
                Status = generation.Status
            }, "VEHICLE_GENERATION_CREATED", "Araç nesli oluşturuldu.");
        }

        public async Task<ServiceResult> UpdateAsync(UpdateVehicleGenerationRequest request)
        {
            var repo = _unitOfWork.GetRepository<VehicleGeneration>();
            var modelRepo = _unitOfWork.GetRepository<VehicleModel>();

            var generation = await repo.GetByIdAsync(request.Id);

            if (generation == null || !generation.Status)
                return ErrorResult("VEHICLE_GENERATION_NOT_FOUND", "Araç nesli bulunamadı.");

            var vehicleModel = await modelRepo.GetWhere(x => x.Id == request.VehicleModelId && x.Status)
                .Include(x => x.Brand)
                .FirstOrDefaultAsync();

            if (vehicleModel == null)
                return ErrorResult("VEHICLE_MODEL_NOT_FOUND", "Araç modeli bulunamadı.");

            generation.VehicleModelId = request.VehicleModelId;
            generation.Name = request.Name;
            generation.Code = request.Code;
            generation.StartYear = request.StartYear;
            generation.EndYear = request.EndYear;
            generation.ImageUrl = request.ImageUrl;
            generation.Description = request.Description;

            repo.Update(generation);
            await _unitOfWork.SaveChangesAsync();

            return SuccessDataResult(new VehicleGenerationDto
            {
                Id = generation.Id,
                VehicleModelId = generation.VehicleModelId,
                VehicleModelName = vehicleModel.Name,
                BrandName = vehicleModel.Brand.Name,
                Name = generation.Name,
                Code = generation.Code,
                StartYear = generation.StartYear,
                EndYear = generation.EndYear,
                ImageUrl = generation.ImageUrl,
                Description = generation.Description,
                Status = generation.Status
            }, "VEHICLE_GENERATION_UPDATED", "Araç nesli güncellendi.");
        }

        public async Task<ServiceResult> DeleteAsync(Guid id)
        {
            var repo = _unitOfWork.GetRepository<VehicleGeneration>();
            var compatibilityRepo = _unitOfWork.GetRepository<ProductCompatibility>();

            var generation = await repo.GetByIdAsync(id);

            if (generation == null || !generation.Status)
                return ErrorResult("VEHICLE_GENERATION_NOT_FOUND", "Araç nesli bulunamadı.");

            if (await compatibilityRepo.AnyAsync(x => x.VehicleGenerationId == id))
                return ErrorResult("VEHICLE_GENERATION_HAS_PRODUCTS", "Bu nesle ait ürün uyumlulukları var. Önce uyumlulukları kaldırın.");

            generation.Status = false;
            repo.Update(generation);
            await _unitOfWork.SaveChangesAsync();

            return SuccessResult("VEHICLE_GENERATION_DELETED", "Araç nesli silindi.");
        }
    }
}
