using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OtoKurSkoda.Application.Dtos;
using OtoKurSkoda.Application.Services.CatalogServices.Interfaces;

namespace OtoKurSkoda.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManufacturerController : ControllerBase
    {
        private readonly IManufacturerService _manufacturerService;

        public ManufacturerController(IManufacturerService manufacturerService)
        {
            _manufacturerService = manufacturerService;
        }

        [HttpPost("list")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _manufacturerService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost("active")]
        public async Task<IActionResult> GetActive()
        {
            var result = await _manufacturerService.GetActiveAsync();
            return Ok(result);
        }

        [HttpPost("get")]
        public async Task<IActionResult> GetById([FromBody] Guid id)
        {
            var result = await _manufacturerService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("get-by-slug")]
        public async Task<IActionResult> GetBySlug([FromBody] string slug)
        {
            var result = await _manufacturerService.GetBySlugAsync(slug);
            return Ok(result);
        }

        [HttpPost("add")]
        [Authorize(Roles = "Manufacturer_Add")]
        public async Task<IActionResult> Create([FromBody] CreateManufacturerRequest request)
        {
            var result = await _manufacturerService.CreateAsync(request);
            return Ok(result);
        }

        [HttpPost("update")]
        [Authorize(Roles = "Manufacturer_Update")]
        public async Task<IActionResult> Update([FromBody] UpdateManufacturerRequest request)
        {
            var result = await _manufacturerService.UpdateAsync(request);
            return Ok(result);
        }

        [HttpPost("delete")]
        [Authorize(Roles = "Manufacturer_Delete")]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            var result = await _manufacturerService.DeleteAsync(id);
            return Ok(result);
        }
    }
}