using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OtoKurSkoda.Application.Dtos;
using OtoKurSkoda.Application.Services.ProductServices.Interfaces;

namespace OtoKurSkoda.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAttributeController : ControllerBase
    {
        private readonly IProductAttributeService _productAttributeService;

        public ProductAttributeController(IProductAttributeService productAttributeService)
        {
            _productAttributeService = productAttributeService;
        }

        [HttpPost("list-by-product")]
        public async Task<IActionResult> GetByProductId([FromBody] Guid productId)
        {
            var result = await _productAttributeService.GetByProductIdAsync(productId);
            return Ok(result);
        }

        [HttpPost("set")]
        [Authorize(Roles = "Product_Update")]
        public async Task<IActionResult> SetAttributes([FromBody] SetProductAttributesRequest request)
        {
            var result = await _productAttributeService.SetAttributesAsync(request);
            return Ok(result);
        }

        [HttpPost("update")]
        [Authorize(Roles = "Product_Update")]
        public async Task<IActionResult> UpdateAttribute([FromBody] UpdateProductAttributeRequest request)
        {
            var result = await _productAttributeService.UpdateAttributeAsync(request);
            return Ok(result);
        }

        [HttpPost("delete")]
        [Authorize(Roles = "Product_Update")]
        public async Task<IActionResult> DeleteAttribute([FromBody] Guid id)
        {
            var result = await _productAttributeService.DeleteAttributeAsync(id);
            return Ok(result);
        }
    }
}