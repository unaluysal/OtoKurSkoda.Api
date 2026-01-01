using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OtoKurSkoda.Application.Defaults;
using OtoKurSkoda.Application.Dtos;
using OtoKurSkoda.Application.Services.UserServices.Interfaces;
using OtoKurSkoda.Domain.Entitys;
using OtoKurSkoda.Infrastructure.UnitOfWork;

namespace OtoKurSkoda.Application.Services.UserServices.Services
{
    public class UserRoleService : BaseApplicationService, IUserRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserRoleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResult> GetUserRoleGroupsAsync(Guid userId)
        {
            var userRepo = _unitOfWork.GetRepository<User>();
            var userRoleRepo = _unitOfWork.GetRepository<UserRole>();

            var user = await userRepo.GetByIdAsync(userId);
            if (user == null || !user.Status)
                return ErrorResult("USER_NOT_FOUND", "Kullanıcı bulunamadı.");

            var userRoles = await userRoleRepo.GetWhere(x => x.UserId == userId && x.Status)
                .Include(x => x.RoleGroup)
                .Where(x => x.RoleGroup.Status)
                .ToListAsync();

            var result = _mapper.Map<List<UserRoleDto>>(userRoles);

            return SuccessListDataResult(result, "USER_ROLES_LISTED", "Kullanıcı rol grupları listelendi.");
        }

        public async Task<ServiceResult> GetUserPermissionsAsync(Guid userId)
        {
            var userRepo = _unitOfWork.GetRepository<User>();

            var user = await userRepo.GetWhere(x => x.Id == userId && x.Status)
                .Include(x => x.UserRoles.Where(ur => ur.Status))
                    .ThenInclude(ur => ur.RoleGroup)
                        .ThenInclude(rg => rg.RoleGroupRoles.Where(rgr => rgr.Status))
                            .ThenInclude(rgr => rgr.Role)
                .FirstOrDefaultAsync();

            if (user == null)
                return ErrorResult("USER_NOT_FOUND", "Kullanıcı bulunamadı.");

            var roleGroups = user.UserRoles
                .Where(ur => ur.RoleGroup.Status)
                .Select(ur => ur.RoleGroup.Name)
                .Distinct()
                .ToList();

            var permissions = user.UserRoles
                .Where(ur => ur.RoleGroup.Status)
                .SelectMany(ur => ur.RoleGroup.RoleGroupRoles)
                .Where(rgr => rgr.Role.Status)
                .Select(rgr => rgr.Role.Name)
                .Distinct()
                .ToList();

            var result = new UserPermissionsDto
            {
                UserId = user.Id,
                Email = user.Email,
                FullName = $"{user.FirstName} {user.LastName}",
                RoleGroups = roleGroups,
                Permissions = permissions
            };

            return SuccessDataResult(result, "USER_PERMISSIONS_LISTED", "Kullanıcı yetkileri listelendi.");
        }

        public async Task<ServiceResult> AssignRoleGroupAsync(AssignRoleGroupToUserRequest request)
        {
            var userRepo = _unitOfWork.GetRepository<User>();
            var roleGroupRepo = _unitOfWork.GetRepository<RoleGroup>();
            var userRoleRepo = _unitOfWork.GetRepository<UserRole>();

            var user = await userRepo.GetByIdAsync(request.UserId);
            if (user == null || !user.Status)
                return ErrorResult("USER_NOT_FOUND", "Kullanıcı bulunamadı.");

            var roleGroup = await roleGroupRepo.GetByIdAsync(request.RoleGroupId);
            if (roleGroup == null || !roleGroup.Status)
                return ErrorResult("ROLE_GROUP_NOT_FOUND", "Rol grubu bulunamadı.");

            if (await userRoleRepo.AnyAsync(x => x.UserId == request.UserId && x.RoleGroupId == request.RoleGroupId && x.Status))
                return ErrorResult("ROLE_GROUP_ALREADY_ASSIGNED", "Bu rol grubu zaten kullanıcıya atanmış.");

            var userRole = new UserRole
            {
                UserId = request.UserId,
                RoleGroupId = request.RoleGroupId
            };

            await userRoleRepo.AddWithoutTokenAsync(userRole);
            await _unitOfWork.SaveChangesAsync();

            return SuccessResult("ROLE_GROUP_ASSIGNED", "Rol grubu kullanıcıya atandı.");
        }

        public async Task<ServiceResult> AssignMultipleRoleGroupsAsync(AssignMultipleRoleGroupsToUserRequest request)
        {
            var userRepo = _unitOfWork.GetRepository<User>();
            var roleGroupRepo = _unitOfWork.GetRepository<RoleGroup>();
            var userRoleRepo = _unitOfWork.GetRepository<UserRole>();

            var user = await userRepo.GetByIdAsync(request.UserId);
            if (user == null || !user.Status)
                return ErrorResult("USER_NOT_FOUND", "Kullanıcı bulunamadı.");

            var existingRoleGroupIds = await userRoleRepo
                .GetWhere(x => x.UserId == request.UserId && x.Status)
                .Select(x => x.RoleGroupId)
                .ToListAsync();

            var assignedCount = 0;

            foreach (var roleGroupId in request.RoleGroupIds)
            {
                if (existingRoleGroupIds.Contains(roleGroupId))
                    continue;

                var roleGroup = await roleGroupRepo.GetByIdAsync(roleGroupId);
                if (roleGroup == null || !roleGroup.Status)
                    continue;

                await userRoleRepo.AddAsync(new UserRole
                {
                    UserId = request.UserId,
                    RoleGroupId = roleGroupId
                });

                assignedCount++;
            }

            await _unitOfWork.SaveChangesAsync();

            return SuccessResult("ROLE_GROUPS_ASSIGNED", $"{assignedCount} rol grubu kullanıcıya atandı.");
        }

        public async Task<ServiceResult> RemoveRoleGroupAsync(Guid userId, Guid roleGroupId)
        {
            var userRoleRepo = _unitOfWork.GetRepository<UserRole>();

            var userRole = await userRoleRepo.GetFirstWhereAsync(x =>
                x.UserId == userId &&
                x.RoleGroupId == roleGroupId &&
                x.Status);

            if (userRole == null)
                return ErrorResult("ASSIGNMENT_NOT_FOUND", "Atama bulunamadı.");

            userRole.Status = false;
            userRoleRepo.Update(userRole);
            await _unitOfWork.SaveChangesAsync();

            return SuccessResult("ROLE_GROUP_REMOVED", "Rol grubu kullanıcıdan kaldırıldı.");
        }

        public async Task<ServiceResult> RemoveAllRoleGroupsAsync(Guid userId)
        {
            var userRoleRepo = _unitOfWork.GetRepository<UserRole>();

            var userRoles = await userRoleRepo
                .GetWhere(x => x.UserId == userId && x.Status)
                .ToListAsync();

            if (!userRoles.Any())
                return ErrorResult("NO_ASSIGNMENTS", "Bu kullanıcıda atanmış rol grubu yok.");

            foreach (var userRole in userRoles)
            {
                userRole.Status = false;
                userRoleRepo.Update(userRole);
            }

            await _unitOfWork.SaveChangesAsync();

            return SuccessResult("ALL_ROLE_GROUPS_REMOVED", $"{userRoles.Count} rol grubu kullanıcıdan kaldırıldı.");
        }
    }
}