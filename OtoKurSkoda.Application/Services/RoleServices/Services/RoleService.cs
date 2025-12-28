using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OtoKurSkoda.Application.Defaults;
using OtoKurSkoda.Application.Dtos;
using OtoKurSkoda.Application.Services.RoleServices.Interfaces;
using OtoKurSkoda.Domain.Entitys;
using OtoKurSkoda.Infrastructure.UnitOfWork;

namespace OtoKurSkoda.Application.Services.RoleServices.Services
{
    public class RoleService : BaseApplicationService, IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResult> GetAllAsync()
        {
            var repo = _unitOfWork.GetRepository<Role>();

            var roles = await repo.GetWhere(x => x.Status)
                .OrderBy(x => x.Name)
                .ToListAsync();

            var result = _mapper.Map<List<RoleDto>>(roles);

            return SuccessListDataResult(result, "ROLES_LISTED", "Roller listelendi.");
        }

        public async Task<ServiceResult> GetByIdAsync(Guid id)
        {
            var repo = _unitOfWork.GetRepository<Role>();

            var role = await repo.GetByIdAsync(id);

            if (role == null || !role.Status)
                return ErrorResult("ROLE_NOT_FOUND", "Rol bulunamadı.");

            var result = _mapper.Map<RoleDto>(role);

            return SuccessDataResult(result, "ROLE_FOUND", "Rol bulundu.");
        }

        public async Task<ServiceResult> CreateAsync(CreateRoleRequest request)
        {
            var repo = _unitOfWork.GetRepository<Role>();

            if (await repo.AnyAsync(x => x.Name == request.Name && x.Status))
                return ErrorResult("ROLE_EXISTS", "Bu isimde bir rol zaten mevcut.");

            var role = new Role
            {
                Name = request.Name,
                Description = request.Description
            };

            await repo.AddAsync(role);
            await _unitOfWork.SaveChangesAsync();

            var result = _mapper.Map<RoleDto>(role);

            return SuccessDataResult(result, "ROLE_CREATED", "Rol oluşturuldu.");
        }

        public async Task<ServiceResult> UpdateAsync(UpdateRoleRequest request)
        {
            var repo = _unitOfWork.GetRepository<Role>();

            var role = await repo.GetByIdAsync(request.Id);

            if (role == null || !role.Status)
                return ErrorResult("ROLE_NOT_FOUND", "Rol bulunamadı.");

            if (await repo.AnyAsync(x => x.Name == request.Name && x.Id != request.Id && x.Status))
                return ErrorResult("ROLE_EXISTS", "Bu isimde bir rol zaten mevcut.");

            role.Name = request.Name;
            role.Description = request.Description;

            repo.Update(role);
            await _unitOfWork.SaveChangesAsync();

            var result = _mapper.Map<RoleDto>(role);

            return SuccessDataResult(result, "ROLE_UPDATED", "Rol güncellendi.");
        }

        public async Task<ServiceResult> DeleteAsync(Guid id)
        {
            var repo = _unitOfWork.GetRepository<Role>();

            var role = await repo.GetByIdAsync(id);

            if (role == null || !role.Status)
                return ErrorResult("ROLE_NOT_FOUND", "Rol bulunamadı.");

            role.Status = false;
            repo.Update(role);
            await _unitOfWork.SaveChangesAsync();

            return SuccessResult("ROLE_DELETED", "Rol silindi.");
        }
    }
}