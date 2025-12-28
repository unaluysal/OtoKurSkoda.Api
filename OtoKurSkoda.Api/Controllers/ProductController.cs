using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OtoKurSkoda.Application.Dtos;
using OtoKurSkoda.Application.Services.ProductServices.Interfaces;

namespace OtoKurSkoda.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("list")]
        public async Task<IActionResult> GetAll([FromBody] ProductFilterRequest filter)
        {
            var result = await _productService.GetAllAsync(filter);
            return Ok(result);
        }

        [HttpPost("get")]
        public async Task<IActionResult> GetById([FromBody] Guid id)
        {
            var result = await _productService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("get-by-slug")]
        public async Task<IActionResult> GetBySlug([FromBody] string slug)
        {
            var result = await _productService.GetBySlugAsync(slug);
            return Ok(result);
        }

        [HttpPost("list-by-category")]
        public async Task<IActionResult> GetByCategory([FromBody] ProductByCategoryRequest request)
        {
            var result = await _productService.GetByCategoryAsync(request.CategoryId, request.PageNumber, request.PageSize);
            return Ok(result);
        }

        [HttpPost("list-by-manufacturer")]
        public async Task<IActionResult> GetByManufacturer([FromBody] ProductByManufacturerRequest request)
        {
            var result = await _productService.GetByManufacturerAsync(request.ManufacturerId, request.PageNumber, request.PageSize);
            return Ok(result);
        }

        [HttpPost("list-by-vehicle")]
        public async Task<IActionResult> GetByVehicle([FromBody] ProductByVehicleRequest request)
        {
            var result = await _productService.GetByVehicleAsync(request.VehicleGenerationId, request.PageNumber, request.PageSize);
            return Ok(result);
        }

        [HttpPost("featured")]
        public async Task<IActionResult> GetFeatured([FromBody] int count = 10)
        {
            var result = await _productService.GetFeaturedAsync(count);
            return Ok(result);
        }

        [HttpPost("new-arrivals")]
        public async Task<IActionResult> GetNewArrivals([FromBody] int count = 10)
        {
            var result = await _productService.GetNewArrivalsAsync(count);
            return Ok(result);
        }

        [HttpPost("best-sellers")]
        public async Task<IActionResult> GetBestSellers([FromBody] int count = 10)
        {
            var result = await _productService.GetBestSellersAsync(count);
            return Ok(result);
        }

        [HttpPost("on-sale")]
        public async Task<IActionResult> GetOnSale([FromBody] int count = 10)
        {
            var result = await _productService.GetOnSaleAsync(count);
            return Ok(result);
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] ProductSearchRequest request)
        {
            var result = await _productService.SearchAsync(request.SearchTerm, request.PageNumber, request.PageSize);
            return Ok(result);
        }

        [HttpPost("add")]
        [Authorize(Roles = "Product_Add")]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
        {
            var result = await _productService.CreateAsync(request);
            return Ok(result);
        }

        [HttpPost("update")]
        [Authorize(Roles = "Product_Update")]
        public async Task<IActionResult> Update([FromBody] UpdateProductRequest request)
        {
            var result = await _productService.UpdateAsync(request);
            return Ok(result);
        }

        [HttpPost("delete")]
        [Authorize(Roles = "Product_Delete")]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            var result = await _productService.DeleteAsync(id);
            return Ok(result);
        }

        [HttpPost("update-stock")]
        [Authorize(Roles = "Product_Update")]
        public async Task<IActionResult> UpdateStock([FromBody] UpdateStockRequest request)
        {
            var result = await _productService.UpdateStockAsync(request.ProductId, request.Quantity);
            return Ok(result);
        }
    }
}