using DealmateApi.Domain.Aggregates;
using DealmateApi.Domain.EntityFilters;
using DealmateApi.Service.PredicateBuilder;
using System.Linq.Expressions;

namespace DealmateApi.Domain.PredicateBuilders;

public class VehicleFilterPredicateBuilder : IPredicateBuilder<VehicleFilter>
{
    public Expression<Func<Vehicle, bool>> BuildPredicate(VehicleFilter filter)
    {
        if (filter.IsEmpty())
        {
            return v => true;
        }
        var predicate = FilterExtensions.CreateDefaultPredicate<Vehicle>();
        predicate = FilterExtensions.BuildCombinedPredicate(filter, () => predicate);
        return predicate;
    }

}