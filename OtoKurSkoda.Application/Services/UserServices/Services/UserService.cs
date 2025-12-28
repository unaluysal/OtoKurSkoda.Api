using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OtoKurSkoda.Application.Defaults;
using OtoKurSkoda.Application.Dtos;
using OtoKurSkoda.Application.Services.UserServices.Interfaces;
using OtoKurSkoda.Domain.Entitys;
using OtoKurSkoda.Infrastructure.UnitOfWork;

namespace OtoKurSkoda.Application.Services.UserServices.Services
{
    public class UserService : BaseApplicationService, IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserRoleService _userRoleService;

        public UserService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IUserRoleService userRoleService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userRoleService = userRoleService;
        }

        public async Task<ServiceResult> GetAllAsync()
        {
            var repo = _unitOfWork.GetRepository<User>();

            var users = await repo.GetWhere(x => x.Status)
                .Include(x => x.UserRoles.Where(ur => ur.Status))
                    .ThenInclude(ur => ur.RoleGroup)
                .OrderByDescending(x => x.CreateTime)
                .ToListAsync();

            var result = users.Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                FirstName = u.FirstName,
                LastName = u.LastName,
                EmailConfirmed = u.EmailConfirmed,
                PhoneConfirmed = u.PhoneConfirmed,
                LastLoginAt = u.LastLoginAt,
                CreateTime = u.CreateTime,
                CreateUserId = u.CreateUserId,
                UpdateTime = u.UpdateTime,
                UpdateUserId = u.UpdateUserId,
                TenatId = u.TenatId,
                Status = u.Status,
                Roles = u.UserRoles
                    .Where(ur => ur.RoleGroup.Status)
                    .Select(ur => ur.RoleGroup.Name)
                    .ToList()
            }).ToList();

            return SuccessListDataResult(result, "USERS_LISTED", "Kullanıcılar listelendi.");
        }

        public async Task<ServiceResult> GetByIdAsync(Guid id)
        {
            var repo = _unitOfWork.GetRepository<User>();

            var user = await repo.GetByIdAsync(id);

            if (user == null || !user.Status)
                return ErrorResult("USER_NOT_FOUND", "Kullanıcı bulunamadı.");

            var result = _mapper.Map<UserDto>(user);

            return SuccessDataResult(result, "USER_FOUND", "Kullanıcı bulundu.");
        }

        public async Task<ServiceResult> GetByIdWithRolesAsync(Guid id)
        {
            var repo = _unitOfWork.GetRepository<User>();

            var user = await repo.GetWhere(x => x.Id == id && x.Status)
                .Include(x => x.UserRoles.Where(ur => ur.Status))
                    .ThenInclude(ur => ur.RoleGroup)
                .FirstOrDefaultAsync();

            if (user == null)
                return ErrorResult("USER_NOT_FOUND", "Kullanıcı bulunamadı.");

            var result = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmailConfirmed = user.EmailConfirmed,
                PhoneConfirmed = user.PhoneConfirmed,
                LastLoginAt = user.LastLoginAt,
                CreateTime = user.CreateTime,
                CreateUserId = user.CreateUserId,
                UpdateTime = user.UpdateTime,
                UpdateUserId = user.UpdateUserId,
                TenatId = user.TenatId,
                Status = user.Status,
                Roles = user.UserRoles
                    .Where(ur => ur.RoleGroup.Status)
                    .Select(ur => ur.RoleGroup.Name)
                    .ToList()
            };

            return SuccessDataResult(result, "USER_FOUND", "Kullanıcı bulundu.");
        }

        public async Task<ServiceResult> CreateAsync(CreateUserRequest request)
        {
            var repo = _unitOfWork.GetRepository<User>();

            // Email kontrolü
            if (await repo.AnyAsync(x => x.Email == request.Email && x.Status))
                return ErrorResult("EMAIL_EXISTS", "Bu email zaten kayıtlı.");

            // Telefon kontrolü
            if (await repo.AnyAsync(x => x.PhoneNumber == request.PhoneNumber && x.Status))
                return ErrorResult("PHONE_EXISTS", "Bu telefon numarası zaten kayıtlı.");

            var user = new User
            {
                Email = request.Email,
                PasswordHash = HashPassword(request.Password),
                PhoneNumber = request.PhoneNumber,
                FirstName = request.FirstName,
                LastName = request.LastName,
                EmailConfirmed = false,
                PhoneConfirmed = false
            };

            await repo.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            // Rolleri ata
            if (request.RoleGroupIds.Any())
            {
                await _userRoleService.AssignMultipleRoleGroupsAsync(new AssignMultipleRoleGroupsToUserRequest
                {
                    UserId = user.Id,
                    RoleGroupIds = request.RoleGroupIds
                });
            }

            var result = _mapper.Map<UserDto>(user);
            result.Roles = request.RoleGroupIds.Any()
                ? await GetRoleGroupNamesAsync(request.RoleGroupIds)
                : new List<string>();

            return SuccessDataResult(result, "USER_CREATED", "Kullanıcı oluşturuldu.");
        }

        public async Task<ServiceResult> UpdateAsync(UpdateUserRequest request)
        {
            var repo = _unitOfWork.GetRepository<User>();

            var user = await repo.GetByIdAsync(request.Id);

            if (user == null || !user.Status)
                return ErrorResult("USER_NOT_FOUND", "Kullanıcı bulunamadı.");

            // Email kontrolü (kendisi hariç)
            if (await repo.AnyAsync(x => x.Email == request.Email && x.Id != request.Id && x.Status))
                return ErrorResult("EMAIL_EXISTS", "Bu email zaten kayıtlı.");

            // Telefon kontrolü (kendisi hariç)
            if (await repo.AnyAsync(x => x.PhoneNumber == request.PhoneNumber && x.Id != request.Id && x.Status))
                return ErrorResult("PHONE_EXISTS", "Bu telefon numarası zaten kayıtlı.");

            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.EmailConfirmed = request.EmailConfirmed;
            user.PhoneConfirmed = request.PhoneConfirmed;

            // Şifre değişecek mi?
            if (!string.IsNullOrEmpty(request.Password))
            {
                user.PasswordHash = HashPassword(request.Password);
            }

            repo.Update(user);
            await _unitOfWork.SaveChangesAsync();

            // Rolleri güncelle (önce hepsini kaldır, sonra yeniden ata)
            await _userRoleService.RemoveAllRoleGroupsAsync(user.Id);

            if (request.RoleGroupIds.Any())
            {
                await _userRoleService.AssignMultipleRoleGroupsAsync(new AssignMultipleRoleGroupsToUserRequest
                {
                    UserId = user.Id,
                    RoleGroupIds = request.RoleGroupIds
                });
            }

            var result = _mapper.Map<UserDto>(user);
            result.Roles = request.RoleGroupIds.Any()
                ? await GetRoleGroupNamesAsync(request.RoleGroupIds)
                : new List<string>();

            return SuccessDataResult(result, "USER_UPDATED", "Kullanıcı güncellendi.");
        }

        public async Task<ServiceResult> DeleteAsync(Guid id)
        {
            var repo = _unitOfWork.GetRepository<User>();

            var user = await repo.GetByIdAsync(id);

            if (user == null || !user.Status)
                return ErrorResult("USER_NOT_FOUND", "Kullanıcı bulunamadı.");

            user.Status = false;
            repo.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return SuccessResult("USER_DELETED", "Kullanıcı silindi.");
        }

        // ═══════════════════════════════════════
        // PRIVATE METHODS
        // ═══════════════════════════════════════
        private static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private async Task<List<string>> GetRoleGroupNamesAsync(List<Guid> roleGroupIds)
        {
            var roleGroupRepo = _unitOfWork.GetRepository<RoleGroup>();

            var roleGroups = await roleGroupRepo
                .GetWhere(x => roleGroupIds.Contains(x.Id) && x.Status)
                .Select(x => x.Name)
                .ToListAsync();

            return roleGroups;
        }
    }
}