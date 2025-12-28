using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OtoKurSkoda.Application.Dtos;
using OtoKurSkoda.Application.Services.VehicleServices.Interfaces;

namespace OtoKurSkoda.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleModelController : ControllerBase
    {
        private readonly IVehicleModelService _vehicleModelService;

        public VehicleModelController(IVehicleModelService vehicleModelService)
        {
            _vehicleModelService = vehicleModelService;
        }

        [HttpPost("list")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _vehicleModelService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost("list-by-brand")]
        public async Task<IActionResult> GetByBrandId([FromBody] Guid brandId)
        {
            var result = await _vehicleModelService.GetByBrandIdAsync(brandId);
            return Ok(result);
        }

        [HttpPost("get")]
        public async Task<IActionResult> GetById([FromBody] Guid id)
        {
            var result = await _vehicleModelService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("add")]
        [Authorize(Roles = "VehicleModel_Add")]
        public async Task<IActionResult> Create([FromBody] CreateVehicleModelRequest request)
        {
            var result = await _vehicleModelService.CreateAsync(request);
            return Ok(result);
        }

        [HttpPost("update")]
        [Authorize(Roles = "VehicleModel_Update")]
        public async Task<IActionResult> Update([FromBody] UpdateVehicleModelRequest request)
        {
            var result = await _vehicleModelService.UpdateAsync(request);
            return Ok(result);
        }

        [HttpPost("delete")]
        [Authorize(Roles = "VehicleModel_Delete")]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            var result = await _vehicleModelService.DeleteAsync(id);
            return Ok(result);
        }
    }
}