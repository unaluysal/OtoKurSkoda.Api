using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OtoKurSkoda.Application.Dtos;
using OtoKurSkoda.Application.Services.CatalogServices.Interfaces;

namespace OtoKurSkoda.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("list")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost("tree")]
        public async Task<IActionResult> GetTree()
        {
            var result = await _categoryService.GetTreeAsync();
            return Ok(result);
        }

        [HttpPost("root")]
        public async Task<IActionResult> GetRootCategories()
        {
            var result = await _categoryService.GetRootCategoriesAsync();
            return Ok(result);
        }

        [HttpPost("children")]
        public async Task<IActionResult> GetChildren([FromBody] Guid parentId)
        {
            var result = await _categoryService.GetChildrenAsync(parentId);
            return Ok(result);
        }

        [HttpPost("get")]
        public async Task<IActionResult> GetById([FromBody] Guid id)
        {
            var result = await _categoryService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("get-by-slug")]
        public async Task<IActionResult> GetBySlug([FromBody] string slug)
        {
            var result = await _categoryService.GetBySlugAsync(slug);
            return Ok(result);
        }

        [HttpPost("add")]
        [Authorize(Roles = "Category_Add")]
        public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request)
        {
            var result = await _categoryService.CreateAsync(request);
            return Ok(result);
        }

        [HttpPost("update")]
        [Authorize(Roles = "Category_Update")]
        public async Task<IActionResult> Update([FromBody] UpdateCategoryRequest request)
        {
            var result = await _categoryService.UpdateAsync(request);
            return Ok(result);
        }

        [HttpPost("delete")]
        [Authorize(Roles = "Category_Delete")]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            var result = await _categoryService.DeleteAsync(id);
            return Ok(result);
        }
    }
}