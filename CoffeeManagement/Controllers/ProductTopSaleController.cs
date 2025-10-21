using CoffeeManagement.Data.Dtos.Report;
using CoffeeManagement.Errors;
using CoffeeManagement.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductTopSaleController : ControllerBase
    {
        private readonly IProductTopSaleService _productTopSaleService;
        private readonly ILogger<ProductTopSaleController> _logger;

        public ProductTopSaleController(IProductTopSaleService productTopSaleService, ILogger<ProductTopSaleController> logger)
        {
            _productTopSaleService = productTopSaleService;
            _logger = logger;
        }

        [HttpGet("{period}")]
        public async Task<IActionResult> GetTopProducts([FromRoute] string period)
        {
            try
            {
                var result = await _productTopSaleService.GetTopProductsAsync(period);
                return Ok(new ApiResponse<IEnumerable<TopSellingProductDto>>(200, "success", result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi thống kê top sản phẩm ({Period})", period);
                return StatusCode(500, new ApiResponse<string>(500, $"Internal Server Error: {ex.Message}"));
            }
        }
    }
}
