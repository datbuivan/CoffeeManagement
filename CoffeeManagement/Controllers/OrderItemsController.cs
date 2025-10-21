using CoffeeManagement.Data.Dtos.Order;
using CoffeeManagement.Data.Entities;
using CoffeeManagement.Errors;
using CoffeeManagement.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;

        public OrderItemsController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingOrderItems()
        {
            var pendingDtos = await _orderItemService.GetPendingOrderItemsAsync();
            return Ok(new ApiResponse<IReadOnlyList<OrderItemResultDto>>(200, "Pending order items retrieved", pendingDtos));
        }

        [HttpPatch("{id}/deliver")]
        public async Task<IActionResult> UpdateIsDeliverOrder(Guid id, [FromBody] bool isDeliver)
        {
            var result = await _orderItemService.UpdateIsDeliverOrderAsync(id, isDeliver);

            if (!result)
                return NotFound(new ApiResponse<string>(404, "OrderItem not found"));

            return Ok(new ApiResponse<bool>(200, "Update successful", isDeliver));
        }

        [HttpPatch("bulk-deliver")]
        public async Task<IActionResult> UpdateIsDeliverOrders([FromBody] BulkUpdateRequest request)
        {
            if (request.Ids == null || !request.Ids.Any())
                return BadRequest(new ApiResponse<string>(400, "No OrderItem IDs provided"));

            var result = await _orderItemService.UpdateIsDeliverOrdersAsync(request.Ids, request.IsDeliver);

            if (!result)
                return NotFound(new ApiResponse<string>(404, "No matching OrderItems found"));

            return Ok(new ApiResponse<int>(200, "Bulk update successful", request.Ids.Count()));
        }

        public class BulkUpdateRequest
        {
            public IEnumerable<Guid> Ids { get; set; } = new List<Guid>();
            public bool IsDeliver { get; set; }
        }
    }
}
