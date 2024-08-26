using DealmateApi.Domain.Aggregates;
using System.Linq.Expressions;

namespace DealmateApi.Service.PredicateBuilder;

public interface IPredicateBuilder<TEntity, TFilter> where TEntity : class
{
    Expression<Func<TEntity, bool>> BuildPredicate(TFilter filter);
}
