using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OtoKurSkoda.Application.Dtos;
using OtoKurSkoda.Application.Services.RoleServices.Interfaces;

namespace OtoKurSkoda.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleGroupController : ControllerBase
    {
        private readonly IRoleGroupService _roleGroupService;

        public RoleGroupController(IRoleGroupService roleGroupService)
        {
            _roleGroupService = roleGroupService;
        }

        [HttpPost("list")]
        [Authorize(Roles = "RoleGroup_List")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _roleGroupService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost("get")]
        [Authorize(Roles = "RoleGroup_Get")]
        public async Task<IActionResult> GetById([FromBody] Guid id)
        {
            var result = await _roleGroupService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("get-with-roles")]
        [Authorize(Roles = "RoleGroup_Get")]
        public async Task<IActionResult> GetWithRoles([FromBody] Guid id)
        {
            var result = await _roleGroupService.GetWithRolesAsync(id);
            return Ok(result);
        }

        [HttpPost("add")]
        [Authorize(Roles = "RoleGroup_Add")]
        public async Task<IActionResult> Create([FromBody] CreateRoleGroupRequest request)
        {
            var result = await _roleGroupService.CreateAsync(request);
            return Ok(result);
        }

        [HttpPost("update")]
        [Authorize(Roles = "RoleGroup_Update")]
        public async Task<IActionResult> Update([FromBody] UpdateRoleGroupRequest request)
        {
            var result = await _roleGroupService.UpdateAsync(request);
            return Ok(result);
        }

        [HttpPost("delete")]
        [Authorize(Roles = "RoleGroup_Delete")]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            var result = await _roleGroupService.DeleteAsync(id);
            return Ok(result);
        }
    }
}