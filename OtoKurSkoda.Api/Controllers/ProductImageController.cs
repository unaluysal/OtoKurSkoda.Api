using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OtoKurSkoda.Application.Dtos;
using OtoKurSkoda.Application.Services.ProductServices.Interfaces;

namespace OtoKurSkoda.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {
        private readonly IProductImageService _productImageService;

        public ProductImageController(IProductImageService productImageService)
        {
            _productImageService = productImageService;
        }

        [HttpPost("list-by-product")]
        public async Task<IActionResult> GetByProductId([FromBody] Guid productId)
        {
            var result = await _productImageService.GetByProductIdAsync(productId);
            return Ok(result);
        }

        [HttpPost("add")]
        [Authorize(Roles = "Product_Update")]
        public async Task<IActionResult> AddImage([FromBody] AddProductImageRequest request)
        {
            var result = await _productImageService.AddImageAsync(request);
            return Ok(result);
        }

        [HttpPost("update")]
        [Authorize(Roles = "Product_Update")]
        public async Task<IActionResult> UpdateImage([FromBody] UpdateProductImageRequest request)
        {
            var result = await _productImageService.UpdateImageAsync(request);
            return Ok(result);
        }

        [HttpPost("delete")]
        [Authorize(Roles = "Product_Update")]
        public async Task<IActionResult> DeleteImage([FromBody] Guid id)
        {
            var result = await _productImageService.DeleteImageAsync(id);
            return Ok(result);
        }

        [HttpPost("set-main")]
        [Authorize(Roles = "Product_Update")]
        public async Task<IActionResult> SetMainImage([FromBody] SetMainImageRequest request)
        {
            var result = await _productImageService.SetMainImageAsync(request.ProductId, request.ImageId);
            return Ok(result);
        }

        [HttpPost("reorder")]
        [Authorize(Roles = "Product_Update")]
        public async Task<IActionResult> ReorderImages([FromBody] ReorderImagesRequest request)
        {
            var result = await _productImageService.ReorderImagesAsync(request.ProductId, request.ImageIds);
            return Ok(result);
        }
    }
}