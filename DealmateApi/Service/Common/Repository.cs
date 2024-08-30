using System.Linq.Expressions;
using System.Reflection;
using DealmateApi.Domain.Aggregates;
using DealmateApi.Infrastructure.DB;
using DealmateApi.Service.Enforcer;
using Microsoft.EntityFrameworkCore;

namespace DealmateApi.Service.Common;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    private IQueryable<T> _query;
    private readonly IEnforcer enforcer;

    public Repository(ApplicationDbContext context, IEnforcer enforcer)
    {
        _context = context;
        this.enforcer = enforcer;
        _query = _context.Set<T>();
        var includeProperties = IncludeAttribute.GetIncludeProperties(typeof(T));

        foreach (var includeProperty in includeProperties)
        {
            _query = _query.Include(includeProperty);
        }
    }

    public async Task InsertDealersBulkAsync(List<T> dealers)
    {
        // Convert list of dealers to JSON
        var dealersJson = JsonConvert.SerializeObject(dealers);

        // Create parameter for stored procedure
        var parameter = new NpgsqlParameter("dealer_data", dealersJson);

        var s = await Database.ExecuteSqlRawAsync("CALL create_dealer_bulk(@dealer_data);", parameter);

        // Execute stored procedure

    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _query.ToListAsync();
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _query.Where(predicate).ToListAsync();
    }

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return await _query.Where(predicate).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
    {
        await _context.Set<T>().AddRangeAsync(entities);
        await _context.SaveChangesAsync();
        return entities;
    }

    public async Task<IEnumerable<T>> RemoveRange(IEnumerable<T> entities)
    {
        _context.Set<T>().RemoveRange(entities);
        await _context.SaveChangesAsync();
        return entities;
    }

    #region API Methods

    public async Task<T?> GetAsync(int id)
    {
        //var permission = GetPermissionForEntity(typeof(T), "Get");
        //await enforcer.EnforceAsync(permission.ToString()!);
        return await _query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
    }

    public async Task<IEnumerable<T>> ListAsync()
    {
        //var permission = GetPermissionForEntity(typeof(T), "List");
        //await enforcer.EnforceAsync(permission.ToString()!);
        return await _query.ToListAsync();
    }

    public async Task<T> AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> Update(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> Remove(T entity)
    {
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    #endregion

    #region Methods

    public static object GetPermissionForEntity(Type entityType, string action)
    {
        if (entityType == null)
        {
            throw new ArgumentNullException(nameof(entityType));
        }

        // Find the static Permissions class
        var permissionsClass = entityType.GetNestedType("Permissions", BindingFlags.Static | BindingFlags.Public);

        if (permissionsClass != null)
        {
            // Get the action property from the Permissions class
            var actionProperty = permissionsClass.GetField(action, BindingFlags.Public | BindingFlags.Static);

            if (actionProperty != null)
            {
                return actionProperty.GetRawConstantValue()!;
            }

            throw new Exception($"Permission action '{action}' not found in '{permissionsClass.Name}' class.");
        }
        throw new Exception("Permissions class not found for this entity.");
    }

    #endregion

}
