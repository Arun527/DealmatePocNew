using DealmateApi.Domain.Aggregates;

namespace DealmateApi.Infrastructure.Interfaces;

public interface IVehicleRepository
{
    Task<IEnumerable<Vehicle>> ExcelUpload(IFormFile file);
    Task<Vehicle> Update(string values,Vehicle vehicle);
    Task<Vehicle> Delete(int id);
}