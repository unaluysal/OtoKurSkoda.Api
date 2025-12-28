using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OtoKurSkoda.Application.Dtos;
using OtoKurSkoda.Application.Services.CatalogServices.Interfaces;

namespace OtoKurSkoda.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttributeDefinitionController : ControllerBase
    {
        private readonly IAttributeDefinitionService _attributeDefinitionService;

        public AttributeDefinitionController(IAttributeDefinitionService attributeDefinitionService)
        {
            _attributeDefinitionService = attributeDefinitionService;
        }

        [HttpPost("list")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _attributeDefinitionService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost("get")]
        public async Task<IActionResult> GetById([FromBody] Guid id)
        {
            var result = await _attributeDefinitionService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("list-by-category")]
        public async Task<IActionResult> GetByCategoryId([FromBody] Guid categoryId)
        {
            var result = await _attributeDefinitionService.GetByCategoryIdAsync(categoryId);
            return Ok(result);
        }

        [HttpPost("filterable")]
        public async Task<IActionResult> GetFilterable()
        {
            var result = await _attributeDefinitionService.GetFilterableAsync();
            return Ok(result);
        }

        [HttpPost("add")]
        [Authorize(Roles = "AttributeDefinition_Add")]
        public async Task<IActionResult> Create([FromBody] CreateAttributeDefinitionRequest request)
        {
            var result = await _attributeDefinitionService.CreateAsync(request);
            return Ok(result);
        }

        [HttpPost("update")]
        [Authorize(Roles = "AttributeDefinition_Update")]
        public async Task<IActionResult> Update([FromBody] UpdateAttributeDefinitionRequest request)
        {
            var result = await _attributeDefinitionService.UpdateAsync(request);
            return Ok(result);
        }

        [HttpPost("delete")]
        [Authorize(Roles = "AttributeDefinition_Delete")]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            var result = await _attributeDefinitionService.DeleteAsync(id);
            return Ok(result);
        }

        [HttpPost("assign-to-category")]
        [Authorize(Roles = "AttributeDefinition_Update")]
        public async Task<IActionResult> AssignToCategory([FromBody] AssignAttributeToCategoryRequest request)
        {
            var result = await _attributeDefinitionService.AssignToCategoryAsync(request.CategoryId, request.AttributeId, request.IsRequired);
            return Ok(result);
        }

        [HttpPost("remove-from-category")]
        [Authorize(Roles = "AttributeDefinition_Update")]
        public async Task<IActionResult> RemoveFromCategory([FromBody] RemoveAttributeFromCategoryRequest request)
        {
            var result = await _attributeDefinitionService.RemoveFromCategoryAsync(request.CategoryId, request.AttributeId);
            return Ok(result);
        }
    }
}