using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OtoKurSkoda.Application.Dtos;
using OtoKurSkoda.Application.Services.VehicleServices.Interfaces;

namespace OtoKurSkoda.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleGenerationController : ControllerBase
    {
        private readonly IVehicleGenerationService _vehicleGenerationService;

        public VehicleGenerationController(IVehicleGenerationService vehicleGenerationService)
        {
            _vehicleGenerationService = vehicleGenerationService;
        }

        [HttpPost("list")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _vehicleGenerationService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost("list-by-model")]
        public async Task<IActionResult> GetByModelId([FromBody] Guid modelId)
        {
            var result = await _vehicleGenerationService.GetByModelIdAsync(modelId);
            return Ok(result);
        }

        [HttpPost("get")]
        public async Task<IActionResult> GetById([FromBody] Guid id)
        {
            var result = await _vehicleGenerationService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("add")]
        [Authorize(Roles = "VehicleGeneration_Add")]
        public async Task<IActionResult> Create([FromBody] CreateVehicleGenerationRequest request)
        {
            var result = await _vehicleGenerationService.CreateAsync(request);
            return Ok(result);
        }

        [HttpPost("update")]
        [Authorize(Roles = "VehicleGeneration_Update")]
        public async Task<IActionResult> Update([FromBody] UpdateVehicleGenerationRequest request)
        {
            var result = await _vehicleGenerationService.UpdateAsync(request);
            return Ok(result);
        }

        [HttpPost("delete")]
        [Authorize(Roles = "VehicleGeneration_Delete")]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            var result = await _vehicleGenerationService.DeleteAsync(id);
            return Ok(result);
        }
    }
}