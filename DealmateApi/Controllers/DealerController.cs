using DealmateApi.Domain.Aggregates;
using DealmateApi.Infrastructure.Interfaces;
using DealmateApi.Infrastructure.Repositories;
using DealmateApi.Service.Common;
using Microsoft.AspNetCore.Mvc;

namespace DealmateApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DealerController : ControllerBase
    {
        private readonly IDealerRepository dealerRepository;
        private readonly IRepository<Dealer> repository;
        public DealerController(IDealerRepository dealerRepository, IRepository<Dealer> repository)
        {
            this.dealerRepository = dealerRepository;
            this.repository = repository;
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

        [HttpPost]
        public async Task<IActionResult> Create(Dealer dealer)
        {
            return Ok(await dealerRepository.Create(dealer));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Dealer dealer)
        {
            return Ok(await dealerRepository.Update(dealer));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await dealerRepository.Delete(id));
        }


        [HttpPost]
        public async Task<IActionResult> FileUpload(IFormFile file)
        {
            return Ok(await dealerRepository.ExcelUpload(file));
        }
    }
}
