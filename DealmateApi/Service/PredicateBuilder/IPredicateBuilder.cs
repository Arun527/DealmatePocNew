using DealmateApi.Domain.Aggregates;
using System.Linq.Expressions;

namespace DealmateApi.Service.PredicateBuilder;

public interface IPredicateBuilder<TFilter>
{
    Expression<Func<Vehicle, bool>> BuildPredicate(TFilter filter);
}
