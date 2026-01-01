using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using OtoKurSkoda.Domain.Defaults;
using OtoKurSkoda.Infrastructure.Context;
using OtoKurSkoda.Infrastructure.Repositories;

namespace OtoKurSkoda.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OtoKurSkodaDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        readonly IConfiguration _configuration;
        public UnitOfWork(OtoKurSkodaDbContext dbContext, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;


            if (dbContext == null)
                throw new ArgumentNullException("dbContext can not be null.");

            _dbContext = dbContext;



            _configuration = configuration;
        }

        #region IUnitOfWork Members
        public IRepository<T> GetRepository<T>() where T : BaseEntity
        {
            return new Repository<T>(_dbContext, _httpContextAccessor, _configuration);
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {

                return await _dbContext.SaveChangesAsync();
            }
            catch
            {

                throw;
            }
        }
        #endregion

        #region IDisposable Members

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }

            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
