using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OtoKurSkoda.Application.Dtos;
using OtoKurSkoda.Application.Services.CartServices.Interfaces;
using System.Security.Claims;

namespace OtoKurSkoda.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        private Guid GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(userIdClaim!);
        }

        [HttpPost("get")]
        public async Task<IActionResult> GetCart()
        {
            var userId = GetUserId();
            var result = await _cartService.GetCartAsync(userId);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            var userId = GetUserId();
            var result = await _cartService.AddToCartAsync(userId, request.ProductId, request.Quantity);
            return Ok(result);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateQuantity([FromBody] UpdateCartItemRequest request)
        {
            var userId = GetUserId();
            var result = await _cartService.UpdateQuantityAsync(userId, request.CartItemId, request.Quantity);
            return Ok(result);
        }

        [HttpPost("remove")]
        public async Task<IActionResult> RemoveFromCart([FromBody] RemoveFromCartRequest request)
        {
            var userId = GetUserId();
            var result = await _cartService.RemoveFromCartAsync(userId, request.CartItemId);
            return Ok(result);
        }

        [HttpPost("clear")]
        public async Task<IActionResult> ClearCart()
        {
            var userId = GetUserId();
            var result = await _cartService.ClearCartAsync(userId);
            return Ok(result);
        }

        [HttpPost("count")]
        public async Task<IActionResult> GetCartItemCount()
        {
            var userId = GetUserId();
            var result = await _cartService.GetCartItemCountAsync(userId);
            return Ok(result);
        }
    }
}
