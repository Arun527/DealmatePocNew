namespace DealmateApi.Service.Repository;

public interface IWriteRepository<T> where T : class
{
    Task<T> AddAsync(T entity);
    Task<T> Update(T entity);
    Task<T> Remove(T entity);
}
