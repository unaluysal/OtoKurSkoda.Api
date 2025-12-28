using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OtoKurSkoda.Application.Defaults;
using OtoKurSkoda.Application.Dtos;
using OtoKurSkoda.Application.Services.RoleServices.Interfaces;
using OtoKurSkoda.Domain.Entitys;
using OtoKurSkoda.Infrastructure.UnitOfWork;

namespace OtoKurSkoda.Application.Services.RoleServices.Services
{
    public class RoleGroupRoleService : BaseApplicationService, IRoleGroupRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoleGroupRoleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResult> GetRolesByGroupIdAsync(Guid roleGroupId)
        {
            var repo = _unitOfWork.GetRepository<RoleGroupRole>();

            var assignments = await repo.GetWhere(x => x.RoleGroupId == roleGroupId && x.Status)
                .Include(x => x.Role)
                .Include(x => x.RoleGroup)
                .Where(x => x.Role.Status)
                .ToListAsync();

            var result = _mapper.Map<List<RoleGroupRoleDto>>(assignments);

            return SuccessListDataResult(result, "ROLES_LISTED", "Roller listelendi.");
        }

        public async Task<ServiceResult> AssignRoleAsync(AssignRoleRequest request)
        {
            var repo = _unitOfWork.GetRepository<RoleGroupRole>();
            var groupRepo = _unitOfWork.GetRepository<RoleGroup>();
            var roleRepo = _unitOfWork.GetRepository<Role>();

            var group = await groupRepo.GetByIdAsync(request.RoleGroupId);
            if (group == null || !group.Status)
                return ErrorResult("ROLE_GROUP_NOT_FOUND", "Rol grubu bulunamadı.");

            var role = await roleRepo.GetByIdAsync(request.RoleId);
            if (role == null || !role.Status)
                return ErrorResult("ROLE_NOT_FOUND", "Rol bulunamadı.");

            if (await repo.AnyAsync(x => x.RoleGroupId == request.RoleGroupId && x.RoleId == request.RoleId && x.Status))
                return ErrorResult("ROLE_ALREADY_ASSIGNED", "Bu rol zaten bu gruba atanmış.");

            var assignment = new RoleGroupRole
            {
                RoleGroupId = request.RoleGroupId,
                RoleId = request.RoleId
            };

            await repo.AddAsync(assignment);
            await _unitOfWork.SaveChangesAsync();

            return SuccessResult("ROLE_ASSIGNED", "Rol gruba atandı.");
        }

        public async Task<ServiceResult> AssignMultipleRolesAsync(AssignMultipleRolesRequest request)
        {
            var repo = _unitOfWork.GetRepository<RoleGroupRole>();
            var groupRepo = _unitOfWork.GetRepository<RoleGroup>();
            var roleRepo = _unitOfWork.GetRepository<Role>();

            var group = await groupRepo.GetByIdAsync(request.RoleGroupId);
            if (group == null || !group.Status)
                return ErrorResult("ROLE_GROUP_NOT_FOUND", "Rol grubu bulunamadı.");

            var existingRoleIds = await repo
                .GetWhere(x => x.RoleGroupId == request.RoleGroupId && x.Status)
                .Select(x => x.RoleId)
                .ToListAsync();

            var assignedCount = 0;

            foreach (var roleId in request.RoleIds)
            {
                if (existingRoleIds.Contains(roleId))
                    continue;

                var role = await roleRepo.GetByIdAsync(roleId);
                if (role == null || !role.Status)
                    continue;

                await repo.AddAsync(new RoleGroupRole
                {
                    RoleGroupId = request.RoleGroupId,
                    RoleId = roleId
                });

                assignedCount++;
            }

            await _unitOfWork.SaveChangesAsync();

            return SuccessResult("ROLES_ASSIGNED", $"{assignedCount} rol gruba atandı.");
        }

        public async Task<ServiceResult> RemoveRoleAsync(Guid roleGroupId, Guid roleId)
        {
            var repo = _unitOfWork.GetRepository<RoleGroupRole>();

            var assignment = await repo.GetFirstWhereAsync(x =>
                x.RoleGroupId == roleGroupId &&
                x.RoleId == roleId &&
                x.Status);

            if (assignment == null)
                return ErrorResult("ASSIGNMENT_NOT_FOUND", "Atama bulunamadı.");

            assignment.Status = false;
            repo.Update(assignment);
            await _unitOfWork.SaveChangesAsync();

            return SuccessResult("ROLE_REMOVED", "Rol gruptan kaldırıldı.");
        }

        public async Task<ServiceResult> RemoveAllRolesAsync(Guid roleGroupId)
        {
            var repo = _unitOfWork.GetRepository<RoleGroupRole>();

            var assignments = await repo
                .GetWhere(x => x.RoleGroupId == roleGroupId && x.Status)
                .ToListAsync();

            if (!assignments.Any())
                return ErrorResult("NO_ASSIGNMENTS", "Bu grupta atanmış rol yok.");

            foreach (var assignment in assignments)
            {
                assignment.Status = false;
                repo.Update(assignment);
            }

            await _unitOfWork.SaveChangesAsync();

            return SuccessResult("ALL_ROLES_REMOVED", $"{assignments.Count} rol gruptan kaldırıldı.");
        }
    }
}