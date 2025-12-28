using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OtoKurSkoda.Application.Dtos;
using OtoKurSkoda.Application.Services.ProductServices.Interfaces;

namespace OtoKurSkoda.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCompatibilityController : ControllerBase
    {
        private readonly IProductCompatibilityService _productCompatibilityService;

        public ProductCompatibilityController(IProductCompatibilityService productCompatibilityService)
        {
            _productCompatibilityService = productCompatibilityService;
        }

        [HttpPost("list-by-product")]
        public async Task<IActionResult> GetByProductId([FromBody] Guid productId)
        {
            var result = await _productCompatibilityService.GetByProductIdAsync(productId);
            return Ok(result);
        }

        [HttpPost("products-by-vehicle")]
        public async Task<IActionResult> GetProductsByVehicle([FromBody] Guid vehicleGenerationId)
        {
            var result = await _productCompatibilityService.GetProductsByVehicleAsync(vehicleGenerationId);
            return Ok(result);
        }

        [HttpPost("add")]
        [Authorize(Roles = "Product_Update")]
        public async Task<IActionResult> AddCompatibility([FromBody] AddProductCompatibilityRequest request)
        {
            var result = await _productCompatibilityService.AddCompatibilityAsync(request);
            return Ok(result);
        }

        [HttpPost("remove")]
        [Authorize(Roles = "Product_Update")]
        public async Task<IActionResult> RemoveCompatibility([FromBody] Guid id)
        {
            var result = await _productCompatibilityService.RemoveCompatibilityAsync(id);
            return Ok(result);
        }

        [HttpPost("set")]
        [Authorize(Roles = "Product_Update")]
        public async Task<IActionResult> SetCompatibilities([FromBody] SetProductCompatibilitiesRequest request)
        {
            var result = await _productCompatibilityService.SetCompatibilitiesAsync(request);
            return Ok(result);
        }
    }
}