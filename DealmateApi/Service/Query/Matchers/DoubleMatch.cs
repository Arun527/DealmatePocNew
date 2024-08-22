using System.Linq.Expressions;

namespace DealmateApi.Service.Query.Matchers;

public class DoubleMatch
{
    public double? Exact { get; set; }
    public double? Min { get; set; }
    public double? Max { get; set; }

    public static DoubleMatch ExactMatch(double value)
        => new() { Exact = value };

    public static DoubleMatch Range(double? min, double? max)
        => new() { Min = min, Max = max };

    public Expression<Func<double, bool>> BuildPredicate()
    {
        Expression<Func<double, bool>> predicate = x => true;

        if (Exact.HasValue)
        {
            predicate = x => x == Exact.Value;
        }
        else if (Min.HasValue || Max.HasValue)
        {
            if (Min.HasValue && Max.HasValue)
            {
                predicate = x => x >= Min.Value && x <= Max.Value;
            }
            else if (Min.HasValue)
            {
                predicate = x => x >= Min.Value;
            }
            else if (Max.HasValue)
            {
                predicate = x => x <= Max.Value;
            }
        }

        return predicate;
    }
}
