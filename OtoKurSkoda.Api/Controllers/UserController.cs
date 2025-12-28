using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OtoKurSkoda.Application.Dtos;
using OtoKurSkoda.Application.Services.UserServices.Interfaces;

namespace OtoKurSkoda.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("list")]
        [Authorize(Roles = "User_List")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _userService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost("get")]
        [Authorize(Roles = "User_Get")]
        public async Task<IActionResult> GetById([FromBody] Guid id)
        {
            var result = await _userService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("get-with-roles")]
        [Authorize(Roles = "User_Get")]
        public async Task<IActionResult> GetByIdWithRoles([FromBody] Guid id)
        {
            var result = await _userService.GetByIdWithRolesAsync(id);
            return Ok(result);
        }

        [HttpPost("add")]
        [Authorize(Roles = "User_Add")]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
        {
            var result = await _userService.CreateAsync(request);
            return Ok(result);
        }

        [HttpPost("update")]
        [Authorize(Roles = "User_Update")]
        public async Task<IActionResult> Update([FromBody] UpdateUserRequest request)
        {
            var result = await _userService.UpdateAsync(request);
            return Ok(result);
        }

        [HttpPost("delete")]
        [Authorize(Roles = "User_Delete")]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            var result = await _userService.DeleteAsync(id);
            return Ok(result);
        }
    }
}