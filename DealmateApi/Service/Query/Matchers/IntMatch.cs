using System.Linq.Expressions;

namespace DealmateApi.Service.Query.Matchers;

public class IntMatch
{
    public int? Exact { get; set; }
    public int? Min { get; set; }
    public int? Max { get; set; }

    public static IntMatch ExactMatch(int value)
        => new() { Exact = value };

    public static IntMatch Range(int? min, int? max)
        => new() { Min = min, Max = max };

    public Expression<Func<int, bool>> BuildPredicate()
    {
        Expression<Func<int, bool>> predicate = x => true;

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
