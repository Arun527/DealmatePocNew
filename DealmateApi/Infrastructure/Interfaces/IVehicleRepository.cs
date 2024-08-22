using DealmateApi.Domain.Aggregates;
using DealmateApi.Domain.EntityFilters;

namespace DealmateApi.Infrastructure.Interfaces;

public interface IVehicleRepository
{
    Task<IEnumerable<Vehicle>> ExcelUpload(IFormFile file);
    Task<Vehicle> Update(Vehicle vehicle);
    Task<Vehicle> Delete(int id);
    Task<List<Vehicle>> QueryListAsync(VehicleFilter filter);
}