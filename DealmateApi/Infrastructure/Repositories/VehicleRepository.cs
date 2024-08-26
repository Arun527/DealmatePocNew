using DealmateApi.Domain.Aggregates;
using DealmateApi.Infrastructure.Interfaces;
using DealmateApi.Service.Common;
using DealmateApi.Service.ExcelProcess;
using DealmateApi.Service.Exceptions;
using DealmateApi.Service.Repository;
using FluentValidation;

namespace DealmateApi.Infrastructure.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly IRepository<Vehicle> readRepository;
    private readonly IWriteRepository<Vehicle> writeRepository;
    private readonly IExcelService excelService;

    public VehicleRepository(IRepository<Vehicle> readRepository, IWriteRepository<Vehicle> writeRepository, IExcelService excelService)
    {
        this.readRepository = readRepository;
        this.writeRepository = writeRepository;
        this.excelService = excelService;
    }

    public async Task<IEnumerable<Vehicle>> ExcelUpload(IFormFile file)
    {
        var vehicleList = excelService.VehicleProcess(file);
        var loadNo = vehicleList.First().LoadNo;
        var existVehicle = await this.readRepository.FindAsync(x => x.LoadNo == loadNo);
        if (existVehicle.Count() != 0)
        {
            throw new ConflictException($"Already the Vehicles LoadNo {loadNo} Uploaded");
        }
        return await this.readRepository.AddRangeAsync(vehicleList);

    }

    public async Task<Vehicle> Update(string values, Vehicle vehicle)
    {
        var paths = FieldMask.CreateFieldMask(values);
        new VehicleValidation().Validator(vehicle,paths);

        var existvehicle = await readRepository.GetByIdAsync(vehicle.Id);
        if (existvehicle == null)
        {
            throw new Exception($"The VehicleID {vehicle.Id} not exist");
        }
        if (paths.Contains(nameof(Vehicle.FrameNo)))
        {
            existvehicle.FrameNo = vehicle.FrameNo;
        }

        existvehicle.FuelType = vehicle.FuelType;
        existvehicle.Key = vehicle.Key;
        existvehicle.SG = vehicle.SG;
        existvehicle.ManufactureDate = vehicle.ManufactureDate;
        existvehicle.Mirror = vehicle.Mirror;
        existvehicle.Tools = vehicle.Tools;
        existvehicle.ManualBook = vehicle.ManualBook;
        existvehicle.Active = vehicle.Active;
        existvehicle = await writeRepository.Update(existvehicle);
        return existvehicle;
    }

    public async Task<Vehicle> Delete(int id)
    {
        var vehicle = await readRepository.GetByIdAsync(id);
        if (vehicle == null)
        {
            throw new Exception($"The vehicle Id:{id} was not found");
        }
        vehicle = await writeRepository.Remove(vehicle);
        return vehicle;
    }

    
}


#region Validation
public class VehicleValidation
{
    private class VehicleUpdateValidator : AbstractValidator<Vehicle>
    {
        private readonly FieldMask _fieldMask;
        public VehicleUpdateValidator(FieldMask fieldMask)
        {
            _fieldMask = fieldMask;
            RuleFor(_ => _fieldMask)
            .NotNull()
            .SetValidator(new FieldMaskValidator(
                nameof(Vehicle.LoadNo),
                nameof(Vehicle.FrameNo)));
            RuleFor(x => x.Id)
           .NotEmpty();

            RuleFor(x => x.LoadNo)
                .NotEmpty()
                .GreaterThan(0)
                .When(_ => _fieldMask.Paths.Contains(nameof(Vehicle.LoadNo)));

            RuleFor(x => x.FrameNo)
                .NotEmpty()
                .When(_ => _fieldMask.Paths.Contains(nameof(Vehicle.FrameNo)));
        }
    }
    private class VehicleCreateValidator : AbstractValidator<Vehicle>
    {
        public VehicleCreateValidator()
        {
            RuleFor(vehicle => vehicle.Id)
                .Empty();
        }
    }

    public void Validator(Vehicle vehicle,FieldMask? fieldMask=null)
    {
        IValidator<Vehicle> validator;
        if (fieldMask != null)
        {
            validator = new VehicleUpdateValidator(fieldMask);
        }
        else
            validator = new VehicleCreateValidator();
        var result = validator.Validate(vehicle);
        if (!result.IsValid)
        {
            throw new ValidationException(result.Errors);
        }
    }

}
#endregion


