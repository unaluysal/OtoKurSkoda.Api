using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OtoKurSkoda.Application.Dtos;
using OtoKurSkoda.Application.Services.UserServices.Interfaces;

namespace OtoKurSkoda.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;

        public UserRoleController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }

        [HttpPost("list")]
        [Authorize(Roles = "UserRole_List")]
        public async Task<IActionResult> GetUserRoleGroups([FromBody] Guid userId)
        {
            var result = await _userRoleService.GetUserRoleGroupsAsync(userId);
            return Ok(result);
        }

        [HttpPost("permissions")]
        [Authorize(Roles = "UserRole_List")]
        public async Task<IActionResult> GetUserPermissions([FromBody] Guid userId)
        {
            var result = await _userRoleService.GetUserPermissionsAsync(userId);
            return Ok(result);
        }

        [HttpPost("assign")]
        [Authorize(Roles = "UserRole_Assign")]
        public async Task<IActionResult> AssignRoleGroup([FromBody] AssignRoleGroupToUserRequest request)
        {
            var result = await _userRoleService.AssignRoleGroupAsync(request);
            return Ok(result);
        }

        [HttpPost("assign-multiple")]
        [Authorize(Roles = "UserRole_Assign")]
        public async Task<IActionResult> AssignMultipleRoleGroups([FromBody] AssignMultipleRoleGroupsToUserRequest request)
        {
            var result = await _userRoleService.AssignMultipleRoleGroupsAsync(request);
            return Ok(result);
        }

        [HttpPost("remove")]
        [Authorize(Roles = "UserRole_Remove")]
        public async Task<IActionResult> RemoveRoleGroup([FromBody] AssignRoleGroupToUserRequest request)
        {
            var result = await _userRoleService.RemoveRoleGroupAsync(request.UserId, request.RoleGroupId);
            return Ok(result);
        }

        [HttpPost("remove-all")]
        [Authorize(Roles = "UserRole_Remove")]
        public async Task<IActionResult> RemoveAllRoleGroups([FromBody] Guid userId)
        {
            var result = await _userRoleService.RemoveAllRoleGroupsAsync(userId);
            return Ok(result);
        }
    }
}