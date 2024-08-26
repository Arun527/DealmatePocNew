using DealmateApi.Infrastructure.DB;
using DealmateApi.Service.Common;
using DealmateApi.Service.Enforcer;
using Microsoft.EntityFrameworkCore;

namespace DealmateApi.Service.Repository
{
    public class WriteRepository<T> : IWriteRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        private IQueryable<T> _query;
        private readonly IEnforcer enforcer;

        public WriteRepository(ApplicationDbContext context, IEnforcer enforcer)
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
    }
}
