using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OtoKurSkoda.Application.Defaults;
using OtoKurSkoda.Application.Dtos;
using OtoKurSkoda.Application.Services.RoleServices.Interfaces;
using OtoKurSkoda.Domain.Entitys;
using OtoKurSkoda.Infrastructure.UnitOfWork;

namespace OtoKurSkoda.Application.Services.RoleServices.Services
{
    public class RoleGroupService : BaseApplicationService, IRoleGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoleGroupService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResult> GetAllAsync()
        {
            var repo = _unitOfWork.GetRepository<RoleGroup>();

            var groups = await repo.GetWhere(x => x.Status)
                .OrderBy(x => x.Name)
                .ToListAsync();

            var result = _mapper.Map<List<RoleGroupDto>>(groups);

            return SuccessListDataResult(result, "ROLE_GROUPS_LISTED", "Rol grupları listelendi.");
        }

        public async Task<ServiceResult> GetByIdAsync(Guid id)
        {
            var repo = _unitOfWork.GetRepository<RoleGroup>();

            var group = await repo.GetByIdAsync(id);

            if (group == null || !group.Status)
                return ErrorResult("ROLE_GROUP_NOT_FOUND", "Rol grubu bulunamadı.");

            var result = _mapper.Map<RoleGroupDto>(group);

            return SuccessDataResult(result, "ROLE_GROUP_FOUND", "Rol grubu bulundu.");
        }

        public async Task<ServiceResult> GetWithRolesAsync(Guid id)
        {
            var repo = _unitOfWork.GetRepository<RoleGroup>();

            var group = await repo.GetWhere(x => x.Id == id && x.Status)
                .Include(x => x.RoleGroupRoles)
                    .ThenInclude(x => x.Role)
                .FirstOrDefaultAsync();

            if (group == null)
                return ErrorResult("ROLE_GROUP_NOT_FOUND", "Rol grubu bulunamadı.");

            var result = _mapper.Map<RoleGroupDto>(group);
            result.Roles = group.RoleGroupRoles
                .Where(x => x.Status && x.Role.Status)
                .Select(x => _mapper.Map<RoleDto>(x.Role))
                .ToList();

            return SuccessDataResult(result, "ROLE_GROUP_FOUND", "Rol grubu bulundu.");
        }

        public async Task<ServiceResult> CreateAsync(CreateRoleGroupRequest request)
        {
            var repo = _unitOfWork.GetRepository<RoleGroup>();

            if (await repo.AnyAsync(x => x.Name == request.Name && x.Status))
                return ErrorResult("ROLE_GROUP_EXISTS", "Bu isimde bir rol grubu zaten mevcut.");

            var group = new RoleGroup
            {
                Name = request.Name,
                Description = request.Description
            };

            await repo.AddAsync(group);
            await _unitOfWork.SaveChangesAsync();

            var result = _mapper.Map<RoleGroupDto>(group);

            return SuccessDataResult(result, "ROLE_GROUP_CREATED", "Rol grubu oluşturuldu.");
        }

        public async Task<ServiceResult> UpdateAsync(UpdateRoleGroupRequest request)
        {
            var repo = _unitOfWork.GetRepository<RoleGroup>();

            var group = await repo.GetByIdAsync(request.Id);

            if (group == null || !group.Status)
                return ErrorResult("ROLE_GROUP_NOT_FOUND", "Rol grubu bulunamadı.");

            if (await repo.AnyAsync(x => x.Name == request.Name && x.Id != request.Id && x.Status))
                return ErrorResult("ROLE_GROUP_EXISTS", "Bu isimde bir rol grubu zaten mevcut.");

            group.Name = request.Name;
            group.Description = request.Description;

            repo.Update(group);
            await _unitOfWork.SaveChangesAsync();

            var result = _mapper.Map<RoleGroupDto>(group);

            return SuccessDataResult(result, "ROLE_GROUP_UPDATED", "Rol grubu güncellendi.");
        }

        public async Task<ServiceResult> DeleteAsync(Guid id)
        {
            var repo = _unitOfWork.GetRepository<RoleGroup>();

            var group = await repo.GetByIdAsync(id);

            if (group == null || !group.Status)
                return ErrorResult("ROLE_GROUP_NOT_FOUND", "Rol grubu bulunamadı.");

            group.Status = false;
            repo.Update(group);
            await _unitOfWork.SaveChangesAsync();

            return SuccessResult("ROLE_GROUP_DELETED", "Rol grubu silindi.");
        }
    }
}