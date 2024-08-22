using System.Linq.Expressions;
using System.Reflection;
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
        var includeProperties = GetIncludeProperties(typeof(T));

        foreach (var includeProperty in includeProperties)
        {
            _query = _query.Include(includeProperty);
        }
    }
    public async Task<List<T>> QueryListAsync(IQueryable<T> query)
    {
        var includeProperties = GetIncludeProperties(typeof(T));
        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }
        return await query.ToListAsync();
    }
    public async Task<List<T>> Test(Expression<Func<T, bool>> predicate)
    {
        _query.Where(predicate);
        return await _query.ToListAsync();
    }
    public IQueryable<T> GetQuery()
        { return _query.AsQueryable(); }

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
    private static IEnumerable<string> GetIncludeProperties(Type type)
    {
        var includeProperties = new List<string>();

        var properties = type.GetProperties()
            .Where(p => p.GetCustomAttribute<IncludeAttribute>() != null);

        foreach (var property in properties)
        {
            includeProperties.Add(property.Name);
            var propertyType = property.PropertyType;

            // Handle nested properties
            if (propertyType.IsClass && propertyType != typeof(string))
            {
                // Recursively get nested includes
                includeProperties.AddRange(GetNestedIncludes(propertyType, property.Name));
            }
        }

        return includeProperties;
    }

    private static IEnumerable<string> GetNestedIncludes(Type type, string parentProperty)
    {
        var nestedIncludes = new List<string>();
        var properties = type.GetProperties()
            .Where(p => p.GetCustomAttribute<IncludeAttribute>() != null);

        foreach (var property in properties)
        {
            var propertyName = property.Name;
            nestedIncludes.Add($"{parentProperty}.{propertyName}");

            var propertyType = property.PropertyType;

            // Handle further nesting
            if (propertyType.IsClass && propertyType != typeof(string))
            {
                nestedIncludes.AddRange(GetNestedIncludes(propertyType, $"{parentProperty}.{propertyName}"));
            }
        }

        return nestedIncludes;
    }

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