using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OtoKurSkoda.Application.Dtos;
using OtoKurSkoda.Application.Services.OrderServices.Interfaces;
using System.Security.Claims;

namespace OtoKurSkoda.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        private Guid GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(userIdClaim!);
        }

        #region Müşteri İşlemleri

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var userId = GetUserId();
            var result = await _orderService.CreateOrderFromCartAsync(userId, request);
            return Ok(result);
        }

        [HttpPost("my-orders")]
        public async Task<IActionResult> GetMyOrders([FromBody] PaginationRequest request)
        {
            var userId = GetUserId();
            var result = await _orderService.GetMyOrdersAsync(userId, request?.PageNumber ?? 1, request?.PageSize ?? 20);
            return Ok(result);
        }

        [HttpPost("my-order-detail")]
        public async Task<IActionResult> GetMyOrderDetail([FromBody] Guid orderId)
        {
            var userId = GetUserId();
            var result = await _orderService.GetMyOrderDetailAsync(userId, orderId);
            return Ok(result);
        }

        [HttpPost("cancel")]
        public async Task<IActionResult> CancelOrder([FromBody] Guid orderId)
        {
            var userId = GetUserId();
            var result = await _orderService.CancelOrderAsync(userId, orderId);
            return Ok(result);
        }

        #endregion

        #region Admin İşlemleri

        [HttpPost("list")]
        [Authorize(Roles = "Order_List")]
        public async Task<IActionResult> GetAllOrders([FromBody] OrderFilterRequest filter)
        {
            var result = await _orderService.GetAllOrdersAsync(filter);
            return Ok(result);
        }

        [HttpPost("get")]
        [Authorize(Roles = "Order_Get")]
        public async Task<IActionResult> GetOrderById([FromBody] Guid orderId)
        {
            var result = await _orderService.GetOrderByIdAsync(orderId);
            return Ok(result);
        }

        [HttpPost("get-by-number")]
        [Authorize(Roles = "Order_Get")]
        public async Task<IActionResult> GetOrderByNumber([FromBody] string orderNumber)
        {
            var result = await _orderService.GetOrderByNumberAsync(orderNumber);
            return Ok(result);
        }

        [HttpPost("update-status")]
        [Authorize(Roles = "Order_Update")]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateOrderStatusRequest request)
        {
            var result = await _orderService.UpdateOrderStatusAsync(request);
            return Ok(result);
        }

        [HttpPost("update-shipping")]
        [Authorize(Roles = "Order_Update")]
        public async Task<IActionResult> UpdateShippingInfo([FromBody] UpdateShippingInfoRequest request)
        {
            var result = await _orderService.UpdateShippingInfoAsync(request);
            return Ok(result);
        }

        [HttpPost("statistics")]
        [Authorize(Roles = "Order_List")]
        public async Task<IActionResult> GetStatistics()
        {
            var result = await _orderService.GetOrderStatisticsAsync();
            return Ok(result);
        }

        #endregion
    }


}
