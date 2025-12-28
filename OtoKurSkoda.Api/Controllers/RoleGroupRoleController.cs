using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OtoKurSkoda.Application.Dtos;
using OtoKurSkoda.Application.Services.RoleServices.Interfaces;

namespace OtoKurSkoda.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleGroupRoleController : ControllerBase
    {
        private readonly IRoleGroupRoleService _roleGroupRoleService;

        public RoleGroupRoleController(IRoleGroupRoleService roleGroupRoleService)
        {
            _roleGroupRoleService = roleGroupRoleService;
        }

        [HttpPost("list")]
        [Authorize(Roles = "RoleGroupRole_List")]
        public async Task<IActionResult> GetRolesByGroupId([FromBody] Guid roleGroupId)
        {
            var result = await _roleGroupRoleService.GetRolesByGroupIdAsync(roleGroupId);
            return Ok(result);
        }

        [HttpPost("assign")]
        [Authorize(Roles = "RoleGroupRole_Assign")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequest request)
        {
            var result = await _roleGroupRoleService.AssignRoleAsync(request);
            return Ok(result);
        }

        [HttpPost("assign-multiple")]
        [Authorize(Roles = "RoleGroupRole_Assign")]
        public async Task<IActionResult> AssignMultipleRoles([FromBody] AssignMultipleRolesRequest request)
        {
            var result = await _roleGroupRoleService.AssignMultipleRolesAsync(request);
            return Ok(result);
        }

        [HttpPost("remove")]
        [Authorize(Roles = "RoleGroupRole_Remove")]
        public async Task<IActionResult> RemoveRole([FromBody] AssignRoleRequest request)
        {
            var result = await _roleGroupRoleService.RemoveRoleAsync(request.RoleGroupId, request.RoleId);
            return Ok(result);
        }

        [HttpPost("remove-all")]
        [Authorize(Roles = "RoleGroupRole_Remove")]
        public async Task<IActionResult> RemoveAllRoles([FromBody] Guid roleGroupId)
        {
            var result = await _roleGroupRoleService.RemoveAllRolesAsync(roleGroupId);
            return Ok(result);
        }
    }
}