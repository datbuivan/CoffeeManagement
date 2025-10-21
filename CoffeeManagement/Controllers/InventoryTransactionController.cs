using CoffeeManagement.Data.Entities;
using CoffeeManagement.DTOs.InventoryTransaction;
using CoffeeManagement.Errors;
using CoffeeManagement.Interface;
using CoffeeManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryTransactionController : ControllerBase
    {
        private readonly IInventoryTransactionService _service;

        public InventoryTransactionController(IInventoryTransactionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var transactions = await _service.Get();
                return Ok(new ApiResponse<IEnumerable<InventoryTransactionResultDto>>(200, "Lấy danh sách giao dịch thành công", transactions));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(400, ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var transaction = await _service.GetById(id);
                return Ok(new ApiResponse<InventoryTransactionResultDto>(200, "Lấy thông tin giao dịch thành công", transaction));
            }
            catch (InvalidOperationException)
            {
                return NotFound(new ApiResponse<object>(404, "Không tìm thấy giao dịch"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(400, ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateInventoryTransactionDto transaction)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                    return BadRequest(new ApiResponse<object>(400, string.Join(", ", errors)));
                }

                var createdTransaction = await _service.Create(transaction);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = createdTransaction.Id },
                    new ApiResponse<InventoryTransactionResultDto>(201, "Tạo giao dịch thành công", createdTransaction));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponse<object>(400, ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(400, ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateInventoryTransactionDto transaction)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                    return BadRequest(new ApiResponse<object>(400, string.Join(", ", errors)));
                }

                var updatedTransaction = await _service.Update(id, transaction);

                return Ok(new ApiResponse<InventoryTransactionResultDto>(200, "Cập nhật giao dịch thành công", updatedTransaction));
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new ApiResponse<object>(404, ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(400, ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _service.Delete(id);
                return Ok(new ApiResponse<object>(200, "Xóa giao dịch thành công"));
            }
            catch (InvalidOperationException)
            {
                return NotFound(new ApiResponse<object>(404, "Không tìm thấy giao dịch"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(400, ex.Message));
            }
        }
    }
}