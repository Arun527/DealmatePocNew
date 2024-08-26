namespace DealmateApi.Service.Repository;

public interface IReadRepository<T, TFilter> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> QueryListAsync(TFilter filter);
}
