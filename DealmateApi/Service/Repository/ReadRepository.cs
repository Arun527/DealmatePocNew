using DealmateApi.Infrastructure.DB;
using DealmateApi.Service.Common;
using DealmateApi.Service.Enforcer;
using DealmateApi.Service.Pagination;
using DealmateApi.Service.PredicateBuilder;
using Microsoft.EntityFrameworkCore;

namespace DealmateApi.Service.Repository;

public class ReadRepository<T, TFilter>: IReadRepository<T, TFilter> where T : class
{
    protected readonly ApplicationDbContext _context;
    private IQueryable<T> _query;
    private readonly IPredicateBuilder<T, TFilter> _predicateBuilder;

    public ReadRepository(ApplicationDbContext context, IPredicateBuilder<T, TFilter> predicateBuilder)
    {
        _predicateBuilder = predicateBuilder;
        _context = context;
        _query = _context.Set<T>();
        var includeProperties = IncludeAttribute.GetIncludeProperties(typeof(T));

        foreach (var includeProperty in includeProperties)
        {
            _query = _query.Include(includeProperty);
        }
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
    }

    public async Task<IEnumerable<T>> QueryListAsync(TFilter filter)
    {
        var predicate = _predicateBuilder.BuildPredicate(filter);
        _query = _query.Where(predicate);
        _query = PaginationExtensions.ApplyPagination(_query, filter);
        return await _query.ToListAsync();
    }
}
