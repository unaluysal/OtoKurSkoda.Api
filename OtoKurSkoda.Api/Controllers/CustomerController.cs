using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OtoKurSkoda.Application.Dtos;
using OtoKurSkoda.Application.Services.UserServices.Interfaces;
using System.Security.Claims;

namespace OtoKurSkoda.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAddressService _addressService;

        public CustomerController(IUserService userService, IAddressService addressService)
        {
            _userService = userService;
            _addressService = addressService;
        }

        private Guid GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(userIdClaim!);
        }

        [HttpPost("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = GetUserId();
            var result = await _userService.GetByIdAsync(userId);
            return Ok(result);
        }

        [HttpPost("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateCustomerProfileRequest request)
        {
            var userId = GetUserId();
            var result = await _userService.UpdateAsync(new UpdateUserRequest
            {
                Id = userId,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                FirstName = request.FirstName,
                LastName = request.LastName
            });
            return Ok(result);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var userId = GetUserId();
            var result = await _userService.ChangePasswordAsync(userId, request.CurrentPassword, request.NewPassword);
            return Ok(result);
        }

        // Address Endpoints
        [HttpPost("addresses")]
        public async Task<IActionResult> GetMyAddresses()
        {
            var userId = GetUserId();
            var result = await _addressService.GetUserAddressesAsync(userId);
            return Ok(result);
        }

        [HttpPost("address/get")]
        public async Task<IActionResult> GetAddress([FromBody] Guid id)
        {
            var userId = GetUserId();
            var result = await _addressService.GetByIdAsync(id, userId);
            return Ok(result);
        }

        [HttpPost("address/create")]
        public async Task<IActionResult> CreateAddress([FromBody] CreateAddressRequest request)
        {
            var userId = GetUserId();
            var result = await _addressService.CreateAsync(userId, request);
            return Ok(result);
        }

        [HttpPost("address/update")]
        public async Task<IActionResult> UpdateAddress([FromBody] UpdateAddressRequest request)
        {
            var userId = GetUserId();
            var result = await _addressService.UpdateAsync(userId, request);
            return Ok(result);
        }

        [HttpPost("address/delete")]
        public async Task<IActionResult> DeleteAddress([FromBody] Guid id)
        {
            var userId = GetUserId();
            var result = await _addressService.DeleteAsync(id, userId);
            return Ok(result);
        }

        [HttpPost("address/set-default")]
        public async Task<IActionResult> SetDefaultAddress([FromBody] Guid id)
        {
            var userId = GetUserId();
            var result = await _addressService.SetDefaultAsync(id, userId);
            return Ok(result);
        }
    }
}
