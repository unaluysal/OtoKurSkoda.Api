using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OtoKurSkoda.Application.Defaults;
using OtoKurSkoda.Application.Dtos;
using OtoKurSkoda.Application.Services.AuthServices.Interfaces;
using OtoKurSkoda.Application.Services.UserServices.Interfaces;
using OtoKurSkoda.Application.Settings;
using OtoKurSkoda.Domain.Entitys;
using OtoKurSkoda.Infrastructure.UnitOfWork;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace OtoKurSkoda.Application.Services.AuthServices.Services
{
    public class AuthService : BaseApplicationService, IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtSettings _jwtSettings;
        private readonly IUserRoleService _userRoleService; 

        public AuthService(
            IUnitOfWork unitOfWork,
            IOptions<JwtSettings> jwtSettings,
            IUserRoleService userRoleService) 
        {
            _unitOfWork = unitOfWork;
            _jwtSettings = jwtSettings.Value;
            _userRoleService = userRoleService; 
        }

        public async Task<ServiceResult> RegisterAsync(RegisterRequest request)
        {
            var userRepo = _unitOfWork.GetRepository<User>();
            var roleGroupRepo = _unitOfWork.GetRepository<RoleGroup>();

            if (await userRepo.AnyAsync(u => u.Email == request.Email))
                return ErrorDataResult<AuthResponse>(null, "EMAIL_EXISTS", "Bu email zaten kayıtlı.");

            if (await userRepo.AnyAsync(u => u.PhoneNumber == request.PhoneNumber))
                return ErrorDataResult<AuthResponse>(null, "PHONE_EXISTS", "Bu telefon numarası zaten kayıtlı.");

            var user = new User
            {
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                PasswordHash = HashPassword(request.Password),
                FirstName = request.FirstName,
                LastName = request.LastName,
                EmailConfirmed = false,
                PhoneConfirmed = false
            };

            await userRepo.AddWithoutTokenAsync(user);
            await _unitOfWork.SaveChangesAsync();  // Önce user'ı kaydet

            // Varsayılan "Customers" rolü ata
            var customerRoleGroup = await roleGroupRepo.GetFirstWhereAsync(rg => rg.Name == "Customers");
            if (customerRoleGroup != null)
            {
                await _userRoleService.AssignRoleGroupAsync(new AssignRoleGroupToUserRequest
                {
                    UserId = user.Id,
                    RoleGroupId = customerRoleGroup.Id
                });
                // AssignRoleGroupAsync içinde zaten SaveChangesAsync var
            }

            return SuccessDataResult(new AuthResponse
            {
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Roles = new List<string> { "Customers" }
                }
            }, "REGISTER_SUCCESS", "Kayıt başarılı.");
        }

        public async Task<ServiceResult> LoginAsync(LoginRequest request, string ipAddress)
        {
            var userRepo = _unitOfWork.GetRepository<User>();
            var refreshTokenRepo = _unitOfWork.GetRepository<RefreshToken>();

            var user = await userRepo.GetFirstWhereAsync(u => u.Email == request.Email);

            if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
                return ErrorDataResult<AuthResponse>(null, "INVALID_CREDENTIALS", "Email veya şifre hatalı.");

            if (!user.Status)
                return ErrorDataResult<AuthResponse>(null, "INACTIVE_USER", "Hesabınız aktif değil.");

            // UserRoleService'den permission'ları al
            var permissionsResult = await _userRoleService.GetUserPermissionsAsync(user.Id);
            var permissions = permissionsResult.Status
                ? ((DataResult<UserPermissionsDto>)permissionsResult).Data
                : null;

            var accessToken = GenerateAccessToken(user, permissions);
            var refreshToken = CreateRefreshToken(user.Id, ipAddress);

            await refreshTokenRepo.AddWithoutTokenAsync(refreshToken);

            user.LastLoginAt = DateTime.UtcNow;
            userRepo.UpdateWithOutToken(user);

            await _unitOfWork.SaveChangesAsync();

            return SuccessDataResult(new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                AccessTokenExpiration = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Roles = permissions?.RoleGroups ?? new List<string>()
                }
            }, "LOGIN_SUCCESS", "Giriş başarılı.");
        }

        public async Task<ServiceResult> RefreshTokenAsync(string token, string ipAddress)
        {
            var refreshTokenRepo = _unitOfWork.GetRepository<RefreshToken>();
            var userRepo = _unitOfWork.GetRepository<User>();

            var refreshToken = await refreshTokenRepo.GetFirstWhereAsync(rt => rt.Token == token);

            if (refreshToken == null)
                return ErrorDataResult<AuthResponse>(null, "INVALID_TOKEN", "Geçersiz token.");

            if (!refreshToken.IsActive)
                return ErrorDataResult<AuthResponse>(null, "TOKEN_EXPIRED", "Token süresi dolmuş veya iptal edilmiş.");

            var user = await userRepo.GetByIdAsync(refreshToken.UserId);

            if (user == null || !user.Status)
                return ErrorDataResult<AuthResponse>(null, "USER_NOT_FOUND", "Kullanıcı bulunamadı.");

            // UserRoleService'den permission'ları al
            var permissionsResult = await _userRoleService.GetUserPermissionsAsync(user.Id);
            var permissions = permissionsResult.Status
                ? ((DataResult<UserPermissionsDto>)permissionsResult).Data
                : null;

            // Eski token'ı iptal et
            refreshToken.IsRevoked = true;
            refreshToken.RevokedAt = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;

            // Yeni token üret
            var newRefreshToken = CreateRefreshToken(user.Id, ipAddress);
            refreshToken.ReplacedByToken = newRefreshToken.Token;

            refreshTokenRepo.UpdateWithOutToken(refreshToken);
            await refreshTokenRepo.AddWithoutTokenAsync(newRefreshToken);

            await _unitOfWork.SaveChangesAsync();

            return SuccessDataResult(new AuthResponse
            {
                AccessToken = GenerateAccessToken(user, permissions),
                RefreshToken = newRefreshToken.Token,
                AccessTokenExpiration = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Roles = permissions?.RoleGroups ?? new List<string>()
                }
            }, "TOKEN_REFRESHED", "Token yenilendi.");
        }

        public async Task<ServiceResult> RevokeTokenAsync(string token, string ipAddress)
        {
            var refreshTokenRepo = _unitOfWork.GetRepository<RefreshToken>();

            var refreshToken = await refreshTokenRepo.GetFirstWhereAsync(rt => rt.Token == token);

            if (refreshToken == null || !refreshToken.IsActive)
                return ErrorResult("INVALID_TOKEN", "Geçersiz token.");

            refreshToken.IsRevoked = true;
            refreshToken.RevokedAt = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;

            refreshTokenRepo.UpdateWithOutToken(refreshToken);
            await _unitOfWork.SaveChangesAsync();

            return SuccessResult("TOKEN_REVOKED", "Çıkış yapıldı.");
        }

        private string GenerateAccessToken(User user, UserPermissionsDto? permissions)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
            };

            // Permission'ları Role claim olarak ekle
            if (permissions?.Permissions != null)
            {
                foreach (var permission in permissions.Permissions)
                {
                    claims.Add(new Claim(ClaimTypes.Role, permission));
                }
            }

            // RoleGroup'ları ayrı claim olarak ekle
            if (permissions?.RoleGroups != null)
            {
                foreach (var roleGroup in permissions.RoleGroups)
                {
                    claims.Add(new Claim("RoleGroup", roleGroup));
                }
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private RefreshToken CreateRefreshToken(Guid userId, string ipAddress)
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            return new RefreshToken
            {
                UserId = userId,
                Token = Convert.ToBase64String(randomBytes),
                ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
                CreatedByIp = ipAddress
            };
        }

        private static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private static bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}