namespace DealmateApi.Domain.Aggregates;
public class Vehicle
{
    public int Id { get; set; }
    public int LoadNo { get; set; }
    public string? FrameNo { get; set; }
    public bool SG { get; set; } = false;
    public bool Mirror { get; set; } = false;
    public bool Tools { get; set; } = false;
    public bool ManualBook { get; set; } = false;
    public string CreatedBy { get; set; } = null!;
    public DateTime CreatedOn { get; set; }
    public string UpdatedBy { get; set; } = null!;
    public DateTime UpdatedOn { get; set; }
    public int? Key { get; set; }
    public double? Mileage { get; set; }
    public string? FuelType { get; set; }
    public DateTime? ManufactureDate { get; set; }
    public TimeSpan? Active { get; set; }

}
