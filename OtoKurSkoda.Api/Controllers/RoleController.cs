using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OtoKurSkoda.Application.Dtos;
using OtoKurSkoda.Application.Services.RoleServices.Interfaces;

namespace OtoKurSkoda.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost("list")]
        [Authorize(Roles = "Role_List")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _roleService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost("get")]
        [Authorize(Roles = "Role_Get")]
        public async Task<IActionResult> GetById([FromBody] Guid id)
        {
            var result = await _roleService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("add")]
        [Authorize(Roles = "Role_Add")]
        public async Task<IActionResult> Create([FromBody] CreateRoleRequest request)
        {
            var result = await _roleService.CreateAsync(request);
            return Ok(result);
        }

        [HttpPost("update")]
        [Authorize(Roles = "Role_Update")]
        public async Task<IActionResult> Update([FromBody] UpdateRoleRequest request)
        {
            var result = await _roleService.UpdateAsync(request);
            return Ok(result);
        }

        [HttpPost("delete")]
        [Authorize(Roles = "Role_Delete")]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            var result = await _roleService.DeleteAsync(id);
            return Ok(result);
        }
    }
}