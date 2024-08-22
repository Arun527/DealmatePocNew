using System.Linq.Expressions;

namespace DealmateApi.Service.Query.Matchers;

public class StringMatch
{
    public string? Exact { get; set; }
    public string? StartsWith { get; set; }
    public string? EndsWith { get; set; }
    public string? Contains { get; set; }


    public static StringMatch ExactMatch(string value)
           => new(){ Exact = value };

    public static StringMatch StartsWithMatch(string value)
        => new() { StartsWith = value };

    public static StringMatch EndsWithMatch(string value)
        => new() { EndsWith = value };

    public static StringMatch ContainsMatch(string value)
        => new() { Contains = value };

    public Expression<Func<string, bool>> BuildPredicate()
    {
        Expression<Func<string, bool>> predicate = str => true;

        if (!string.IsNullOrEmpty(Exact))
        {
            var exactValue = Exact.ToLower();
            predicate = str => str != null && str.ToLower() == exactValue;
        }
        else if (!string.IsNullOrEmpty(StartsWith))
        {
            var startsWithValue = StartsWith.ToLower();
            predicate = str => str != null && str.ToLower().StartsWith(startsWithValue);
        }
        else if (!string.IsNullOrEmpty(EndsWith))
        {
            var endsWithValue = EndsWith.ToLower();
            predicate = str => str != null && str.ToLower().EndsWith(endsWithValue);
        }
        else if (!string.IsNullOrEmpty(Contains))
        {
            var containsValue = Contains.ToLower();
            predicate = str => str != null && str.ToLower().Contains(containsValue);
        }

        return predicate;
    }
}
