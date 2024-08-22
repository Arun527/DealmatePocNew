using System;
using System.Linq.Expressions;

namespace DealmateApi.Service.Query.Matchers;

public class DateTimeMatch
{
    public class AbsoluteDateTime
    {
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? Day { get; set; }
        public int? Hour { get; set; }
        public int? Minute { get; set; }
    }

    public DateTime? Exact { get; set; }
    public DateTime? Before { get; set; }
    public DateTime? OnOrAfter { get; set; }
    public AbsoluteDateTime? Absolute { get; set; }

    public static DateTimeMatch ExactMatch(DateTime value)
        => new() { Exact = value };

    public static DateTimeMatch BeforeMatch(DateTime value)
        => new() { Before = value };

    public static DateTimeMatch OnOrAfterMatch(DateTime value)
        => new() { OnOrAfter = value };
    public static DateTimeMatch RangeMatch(DateTime before, DateTime after)
        => new() {Before = before, OnOrAfter = after };

    public static DateTimeMatch AbsoluteMatch(AbsoluteDateTime absolute)
    {
        var match = new DateTimeMatch();
        if (absolute.Year.HasValue || absolute.Month.HasValue || absolute.Day.HasValue ||
            absolute.Hour.HasValue || absolute.Minute.HasValue)
        {
            match.Absolute = absolute;
        }
        return match;
    }
    public Expression<Func<DateTime, bool>> BuildPredicate()
    {
        Expression<Func<DateTime, bool>> predicate = date => true;

        if (Exact.HasValue)
        {
            predicate = date => date == Exact.Value;
        }
        else if (Before.HasValue || OnOrAfter.HasValue)
        {
            if (Before.HasValue && OnOrAfter.HasValue)
            {
                predicate = date => date < Before.Value && date >= OnOrAfter.Value;
            }
            else if (Before.HasValue)
            {
                predicate = date => date < Before.Value;
            }
            else if (OnOrAfter.HasValue)
            {
                predicate = date => date >= OnOrAfter.Value;
            }
        }
        else if (Absolute != null)
        {
            var range = ConstructDateTimeRange(Absolute);
            predicate = date => date < DateTime.SpecifyKind(range.End, DateTimeKind.Utc) && date >= DateTime.SpecifyKind(range.Start, DateTimeKind.Utc);
        }
        return predicate;
    }
    private (DateTime Start, DateTime End) ConstructDateTimeRange(AbsoluteDateTime absolute)
    {
        int year = absolute.Year ?? 1;
        int month = absolute.Month ?? 1;
        int day = absolute.Day ?? 1;
        int hour = absolute.Hour ?? 0;
        int minute = absolute.Minute ?? 0;
        DateTime start = new();
        DateTime end = new();
        if (month < 1 || month > 12) throw new Exception("invalid");
        if (day < 1 || day > 31) throw new Exception("invalid");
        if (hour < 0 || hour > 23) throw new Exception("invalid");
        if (minute < 0 || minute > 59) throw new Exception("invalid");
        if(absolute.Minute.HasValue)
        {
            start = (absolute.Hour != null && absolute.Day != null && absolute.Month != null && absolute.Year != null)?
            new DateTime(year, month, day, hour, minute, 0): throw new Exception("invalid");
            end = start.AddMinutes(1);
            return (start, end);
        }
        else if (absolute.Hour.HasValue)
        {
            start = (absolute.Day != null && absolute.Month != null && absolute.Year != null) ?
            new DateTime(year, month, day, hour, 0, 0) : throw new Exception("invalid");
            end = start.AddHours(1);
            return (start, end);
        }
        else if (absolute.Day.HasValue)
        {
            start = (absolute.Month != null && absolute.Year != null) ?
            new DateTime(year, month, day, 0, 0, 0) : throw new Exception("invalid");
            end = start.AddDays(1);
            return (start, end);
        }
        else if (absolute.Month.HasValue)
        {
            start = (absolute.Year != null) ?
            new DateTime(year, month, 1, 0, 0, 0) : throw new Exception("invalid");
            end = start.AddMonths(1);
            return (start, end);
        }
        else if (absolute.Year.HasValue)
        {
            start = new DateTime(year, 1, 1, 0, 0, 0);
            end = start.AddYears(1);
            return (start, end);
        }
        return (start, end);
    }

}

