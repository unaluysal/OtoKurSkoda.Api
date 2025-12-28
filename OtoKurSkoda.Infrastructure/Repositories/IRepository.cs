using OtoKurSkoda.Domain.Defaults;
using System.Linq.Expressions;

namespace OtoKurSkoda.Infrastructure.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {

        Task<bool> AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        bool Update(T entity);
        void UpdateRange(IEnumerable<T> entities);

        bool Delete(T entity);
        Task<bool> DeleteByIdAsync(Guid id);
        void DeleteRange(IEnumerable<T> entities);

        IQueryable<T> GetAll();
        IQueryable<T> GetWhere(Expression<Func<T, bool>> expression);
        Task<T> GetByIdAsync(Guid id);
        Task<T> GetFirstWhereAsync(Expression<Func<T, bool>> expression);
        Task<T> GetFirstAsync();
        Task<bool> AnyAsync();
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);



        //Token olmadan

        bool UpdateWithOutToken(T entity);
        Task<bool> AddWithoutTokenAsync(T entity);

    }
}
