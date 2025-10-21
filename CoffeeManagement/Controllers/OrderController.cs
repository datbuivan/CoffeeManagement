using AutoMapper;
using CoffeeManagement.Data.Dtos;
using CoffeeManagement.Data.Dtos.Order;
using CoffeeManagement.Errors;
using CoffeeManagement.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public OrderController(
             IOrderService orderService,
             IUnitOfWork uow,
             IConfiguration config,
             IMapper mapper)
        {
            _orderService = orderService;
            _uow = uow;
            _config = config;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var orderDto = await _orderService.GetById(id);
                return Ok(new ApiResponse<OrderResultDto>(200, "Lấy dữ liệu thành công", orderDto));
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new ApiResponse<object>(404, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(500, "Có lỗi xảy ra: " + ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAndPayOrder([FromBody] OrderAndPayDto dto)
        {
            try
            {
                var result = await _orderService.CreateAndPayOrderAsync(dto);
                // Kết quả có thể là OrderResultDto hoặc { PaymentUrl = "..." }
                return Ok(new ApiResponse<object>(200, data: result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(400, ex.Message));
            }
        }

        [HttpGet("vnpay-return")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status302Found)] // Redirect
        public async Task<IActionResult> VnPayReturn()
        {
            try
            {
                await _orderService.HandleVnPayCallbackAsync(Request.Query);

                var frontendUrl = _config["App:FrontendUrl"];
                if (string.IsNullOrEmpty(frontendUrl))
                    return BadRequest(new ApiResponse<object>(400, "FrontendUrl is not configured."));

                var orderId = Request.Query["vnp_TxnRef"];
                var responseCode = Request.Query["vnp_ResponseCode"];

                if (responseCode == "00")
                {
                    return Redirect($"{frontendUrl}/payment/success");
                }
                else
                {
                    return Redirect($"{frontendUrl}/payment/failed");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(400, ex.Message));
            }
        }
    }
}