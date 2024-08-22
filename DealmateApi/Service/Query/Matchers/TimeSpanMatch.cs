using System.Linq.Expressions;

namespace DealmateApi.Service.Query.Matchers;

public class TimeSpanMatch
{
    public class AbsoluteTime
    {
        public int? Day { get; set; }
        public int? Hour { get; set; }
        public int? Minute { get; set; }
        public int? Second { get; set; }
    }

    public AbsoluteTime? AtLeast { get; set; }
    public AbsoluteTime? Lessthan { get; set; }


    public Expression<Func<TimeSpan, bool>> BuildPredicate()
    {
        Expression<Func<TimeSpan, bool>> predicate = x => true;

        if (AtLeast != null && Lessthan != null)
        {
            var atLeast = ConstructTime(AtLeast);
            var lessThan = ConstructTime(AtLeast);
            predicate = x => x >= atLeast && x < lessThan;

        }
        else if (AtLeast != null)
        {
            var atLeast = ConstructTime(AtLeast);
            predicate = x => x >= atLeast;
        }
        else if (Lessthan != null)
        {
            var lessThan = ConstructTime(Lessthan);
            predicate = x => x < lessThan;
        }
        return predicate;
    }
    private TimeSpan ConstructTime(AbsoluteTime absolute)
    {
        return new TimeSpan(absolute.Day??0, absolute.Hour ?? 0, absolute.Minute ?? 0, absolute.Second ?? 0);
    }

}
