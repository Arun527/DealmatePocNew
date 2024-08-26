using DealmateApi.Domain.Aggregates;
using DealmateApi.Domain.EntityFilters;
using DealmateApi.Infrastructure.Interfaces;
using DealmateApi.Service.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DealmateApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleRepository vehicleRepository;       
        private readonly IReadRepository<Vehicle,VehicleFilter> repository;

        public VehicleController(IVehicleRepository vehicleRepository, IReadRepository<Vehicle, VehicleFilter> repository)
        {
            this.vehicleRepository = vehicleRepository;
            this.repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> List([FromBody]VehicleFilter filter)
        {
            return Ok(await repository.QueryListAsync(filter));
        }

      
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await repository.GetByIdAsync(id));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromQuery]string paths, [FromBody] Vehicle vehicle)
        {
            return Ok(await vehicleRepository.Update(paths,vehicle));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await vehicleRepository.Delete(id));
        }

        [HttpPost]
        public async Task<IActionResult> FileUpload(IFormFile file)
        {
            return Ok(await vehicleRepository.ExcelUpload(file));
        }
    }
}
