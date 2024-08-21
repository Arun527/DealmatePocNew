using DealmateApi.Domain.Aggregates;
using DealmateApi.Infrastructure.Interfaces;
using DealmateApi.Service.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DealmateApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleRepository vehicleRepository;       
        private readonly IRepository<Vehicle> repository;
        private readonly ILogger<VehicleController> _logger;

        public VehicleController(IVehicleRepository vehicleRepository, IRepository<Vehicle> repository
            ,ILogger<VehicleController> logger)
        {
            this.vehicleRepository = vehicleRepository;
            this.repository = repository;
            _logger = logger;   
        }
      
        [HttpGet]
        public async Task<IActionResult> List()
        {
            return Ok(await repository.ListAsync());
        }

      
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await repository.GetAsync(id));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Vehicle vehicle)
        {
            return Ok(await vehicleRepository.Update(vehicle));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await vehicleRepository.Delete(id));
        }

        [HttpPost]
        public async Task<IActionResult> FileUpload(IFormFile file)
        {
            _logger.LogInformation("Controller received a file with name: {FileName}, size: {FileSize} bytes", file.FileName, file.Length);
            return Ok(await vehicleRepository.ExcelUpload(file));
        }
    }
}
