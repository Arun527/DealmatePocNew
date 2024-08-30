using DealmateApi.Domain.Aggregates;

namespace DealmateApi.Service.ExcelProcess;

public interface IExcelService
{
    List<Vehicle> VehicleProcess(IFormFile file);
}
