using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using OtoKurSkoda.Domain.Defaults;
using OtoKurSkoda.Infrastructure.Context;
using System.Linq.Expressions;
using System.Security.Claims;

namespace OtoKurSkoda.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly OtoKurSkodaDbContext _dbcontext;
        private readonly DbSet<T> _dbSet;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public Repository(OtoKurSkodaDbContext OtoKurSkodaDbContext, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _dbcontext = OtoKurSkodaDbContext;
            _dbSet = _dbcontext.Set<T>();
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        /// <summary>
        /// JWT token'dan kullanıcı ID'sini alır
        /// </summary>
        private Guid GetCurrentUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            
            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return userId;
            }
            
            return Guid.Empty;
        }

        /// <summary>
        /// Config'den TenantId'yi alır
        /// </summary>
        private Guid GetTenantId()
        {
            var tenantIdStr = _configuration.GetSection("TenatId").Value;
            return Guid.TryParse(tenantIdStr, out var tenantId) ? tenantId : Guid.Empty;
        }

        public async Task<bool> AddAsync(T entity)
        {
            entity.CreateTime = DateTime.Now;
            entity.CreateUserId = GetCurrentUserId();
            entity.Status = true;
            entity.TenatId = GetTenantId();
            
            var res = await _dbSet.AddAsync(entity);
            return res.State == EntityState.Added;
        }

        public async Task<bool> AddWithoutTokenAsync(T entity)
        {
            entity.CreateTime = DateTime.Now;
            entity.CreateUserId = Guid.Empty;
            entity.Status = true;
            entity.TenatId = GetTenantId();
            
            var res = await _dbSet.AddAsync(entity);
            return res.State == EntityState.Added;
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            var userId = GetCurrentUserId();
            var tenantId = GetTenantId();

            foreach (var entity in entities)
            {
                entity.CreateTime = DateTime.Now;
                entity.CreateUserId = userId;
                entity.Status = true;
                entity.TenatId = tenantId;
            }

            await _dbSet.AddRangeAsync(entities);
        }

        public bool Delete(T entity)
        {
            EntityEntry<T> entityEntry = _dbSet.Remove(entity);
            return entityEntry.State == EntityState.Deleted;
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return false;
            return Delete(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet.AsQueryable();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<T> GetFirstAsync()
        {
            return await _dbSet.FirstOrDefaultAsync();
        }

        public async Task<T> GetFirstWhereAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.FirstOrDefaultAsync(expression);
        }

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression);
        }

        public bool Update(T entity)
        {
            entity.UpdateTime = DateTime.Now;
            entity.UpdateUserId = GetCurrentUserId();

            EntityEntry<T> entityEntry = _dbSet.Update(entity);
            entityEntry.Property(e => e.CreateTime).IsModified = false;
            entityEntry.Property(e => e.CreateUserId).IsModified = false;
            entityEntry.Property(e => e.TenatId).IsModified = false;
            
            return entityEntry.State == EntityState.Modified;
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            var userId = GetCurrentUserId();

            foreach (var entity in entities)
            {
                entity.UpdateTime = DateTime.Now;
                entity.UpdateUserId = userId;

                EntityEntry<T> entityEntry = _dbSet.Update(entity);
                entityEntry.Property(e => e.CreateTime).IsModified = false;
                entityEntry.Property(e => e.CreateUserId).IsModified = false;
                entityEntry.Property(e => e.TenatId).IsModified = false;
            }
        }

        public async Task<bool> AnyAsync()
        {
            return await _dbSet.AnyAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.AnyAsync(expression);
        }

        public bool UpdateWithOutToken(T entity)
        {
            entity.UpdateTime = DateTime.Now;
            entity.UpdateUserId = Guid.Empty;

            EntityEntry<T> entityEntry = _dbSet.Update(entity);
            entityEntry.Property(e => e.CreateTime).IsModified = false;
            entityEntry.Property(e => e.CreateUserId).IsModified = false;
            entityEntry.Property(e => e.TenatId).IsModified = false;
            
            return entityEntry.State == EntityState.Modified;
        }
    }
}
