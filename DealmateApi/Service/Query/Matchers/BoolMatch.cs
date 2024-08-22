using System.Linq.Expressions;

namespace DealmateApi.Service.Query.Matchers;

public class BoolMatch
{
    public enum MatchType
    {
        Unspecified = 0,
        True = 1,
        False = 2,
        Null = 3,
        NotNull = 4
    }

    public MatchType IsType { get; set; }

    public static BoolMatch True => new() { IsType = MatchType.True };
    public static BoolMatch False => new() { IsType = MatchType.False };
    public static BoolMatch NotNull => new() { IsType = MatchType.NotNull };
    public static BoolMatch Null => new() { IsType = MatchType.Null };
    public static BoolMatch Unspecified => new() { IsType = MatchType.Unspecified };

    public BoolMatch()
    {
        IsType = MatchType.Unspecified; // Default value
    }

    public BoolMatch(MatchType type)
    {
        IsType = type;
    }

    public static Expression<Func<bool, bool>> BuildPredicate(BoolMatch boolMatch)
    {
        return boolMatch.IsType switch
        {
            MatchType.True => _ => _,
            MatchType.False => _ => !_,
            MatchType.Null => _ => false,
            MatchType.NotNull => _ => true, 
            _ => _ => false // Default case
        };
    }
}