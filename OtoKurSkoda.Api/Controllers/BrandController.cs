using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OtoKurSkoda.Application.Dtos;
using OtoKurSkoda.Application.Services.VehicleServices.Interfaces;

namespace OtoKurSkoda.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpPost("list")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _brandService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost("active")]
        public async Task<IActionResult> GetActive()
        {
            var result = await _brandService.GetActiveAsync();
            return Ok(result);
        }

        [HttpPost("get")]
        public async Task<IActionResult> GetById([FromBody] Guid id)
        {
            var result = await _brandService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("get-by-slug")]
        public async Task<IActionResult> GetBySlug([FromBody] string slug)
        {
            var result = await _brandService.GetBySlugAsync(slug);
            return Ok(result);
        }

        [HttpPost("add")]
        [Authorize(Roles = "Brand_Add")]
        public async Task<IActionResult> Create([FromBody] CreateBrandRequest request)
        {
            var result = await _brandService.CreateAsync(request);
            return Ok(result);
        }

        [HttpPost("update")]
        [Authorize(Roles = "Brand_Update")]
        public async Task<IActionResult> Update([FromBody] UpdateBrandRequest request)
        {
            var result = await _brandService.UpdateAsync(request);
            return Ok(result);
        }

        [HttpPost("delete")]
        [Authorize(Roles = "Brand_Delete")]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            var result = await _brandService.DeleteAsync(id);
            return Ok(result);
        }
    }
}