using System.Linq.Expressions;

namespace DealmateApi.Service.Common;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
    Task<IEnumerable<T>> RemoveRange(IEnumerable<T> entities);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    IQueryable<T> GetQuery();
    Task<List<T>> QueryListAsync(IQueryable<T> query);

    #region API Methods
    Task<T?> GetAsync(int id);
    Task<IEnumerable<T>> ListAsync();
    Task<T> AddAsync(T entity);
    Task<T> Update(T entity);
    Task<T> Remove(T entity);

    #endregion
}
