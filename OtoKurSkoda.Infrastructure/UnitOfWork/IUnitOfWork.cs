using OtoKurSkoda.Domain.Defaults;
using OtoKurSkoda.Infrastructure.Repositories;

namespace OtoKurSkoda.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> GetRepository<T>() where T : BaseEntity;
        Task<int> SaveChangesAsync();
    }
}
