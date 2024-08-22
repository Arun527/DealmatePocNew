using DealmateApi.Service.Query.Matchers;

namespace DealmateApi.Domain.EntityFilters;

public class VehicleFilter
{
    public IEnumerable<StringMatch> FuelType { get; set; } = Enumerable.Empty<StringMatch>();
    public IEnumerable<StringMatch> FrameNo { get; set; } = Enumerable.Empty<StringMatch>();
    public IEnumerable<IntMatch> Key { get; set; } = Enumerable.Empty<IntMatch>();
    public IEnumerable<DoubleMatch> Mileage { get; set; } = Enumerable.Empty<DoubleMatch>();
    public BoolMatch SG { get; set; } = new BoolMatch(BoolMatch.MatchType.Unspecified);
    public BoolMatch Mirror { get; set; } = new BoolMatch(BoolMatch.MatchType.Unspecified);
    public BoolMatch ManualBook { get; set; } = new BoolMatch(BoolMatch.MatchType.Unspecified);
    public BoolMatch Tools { get; set; } = new BoolMatch(BoolMatch.MatchType.Unspecified); 
    public IEnumerable<DateTimeMatch> ManufactureDate { get; set; } = Enumerable.Empty<DateTimeMatch>();
    public IEnumerable<TimeSpanMatch> Active { get; set; } = Enumerable.Empty<TimeSpanMatch>();
}
