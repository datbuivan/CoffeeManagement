using CoffeeManagement.Data.Dtos.Report;
using CoffeeManagement.Errors;
using CoffeeManagement.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CoffeeManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderReportController : ControllerBase
    {
        private readonly IOrderReportService _orderReportService;
        private readonly ILogger<OrderReportController> _logger;

        public OrderReportController(
            IOrderReportService orderReportService,
            ILogger<OrderReportController> logger)
        {
            _orderReportService = orderReportService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN,MANAGER,STAFF")]
        public async Task<IActionResult> GetOrderReport([FromQuery] OrderReportRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse<object>(400, "Dữ liệu không hợp lệ", ModelState));
                }

                var (report, summary, _) = await _orderReportService.GetOrderReportAsync(
                    request.FromDate,
                    request.ToDate,
                    isExportPdf: false
                );

                var response = new OrderReportResponseDto
                {
                    Report = report,
                    Summary = summary
                };

                return Ok(new ApiResponse<OrderReportResponseDto>(200, "Lấy báo cáo thành công", response));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid input for order report");
                return BadRequest(new ApiResponse<object>(400, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không mong đợi khi lấy báo cáo đơn hàng");
                return StatusCode(500, new ApiResponse<object>(500, "Đã xảy ra lỗi không mong đợi. Vui lòng thử lại sau."));
            }
        }

        [HttpGet("export-pdf")]
        [Authorize(Roles = "ADMIN,MANAGER,STAFF")]
        public async Task<IActionResult> ExportPdf([FromQuery] OrderReportRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse<object>(400, "Dữ liệu không hợp lệ", ModelState));
                }

                var (report, summary, pdfFile) = await _orderReportService.GetOrderReportAsync(
                    request.FromDate,
                    request.ToDate,
                    isExportPdf: true
                );

                if (pdfFile == null || pdfFile.Length == 0)
                {
                    return StatusCode(500, new ApiResponse<object>(500, "Không thể tạo file PDF"));
                }

                var fileName = $"BaoCaoDonHang_{request.FromDate:yyyyMMdd}_{request.ToDate:yyyyMMdd}.pdf";
                return File(pdfFile, "application/pdf", fileName);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid input for PDF export");
                return BadRequest(new ApiResponse<object>(400, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không mong đợi khi xuất PDF báo cáo đơn hàng");
                return StatusCode(500, new ApiResponse<object>(500, "Đã xảy ra lỗi không mong đợi khi xuất file PDF."));
            }
        }

        [HttpGet("monthly/{year}/{month}")]
        public async Task<IActionResult> GetMonthlyReport(int year, int month)
        {
            try
            {
                if (month < 1 || month > 12)
                {
                    return BadRequest(new ApiResponse<object>(400, "Tháng phải từ 1 đến 12"));
                }

                if (year < 2000 || year > DateTime.Now.Year + 1)
                {
                    return BadRequest(new ApiResponse<object>(400, "Năm không hợp lệ"));
                }

                var fromDate = new DateTime(year, month, 1);
                var toDate = fromDate.AddMonths(1).AddDays(-1);

                var (report, summary, _) = await _orderReportService.GetOrderReportAsync(fromDate, toDate);

                var response = new OrderReportResponseDto
                {
                    Report = report,
                    Summary = summary
                };

                return Ok(new ApiResponse<OrderReportResponseDto>(200, "Lấy báo cáo tháng thành công", response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy báo cáo tháng {Month}/{Year}", month, year);
                return StatusCode(500, new ApiResponse<object>(500, "Đã xảy ra lỗi không mong đợi."));
            }
        }

        [HttpGet("monthly/{year}/{month}/export-pdf")]
        [Authorize(Roles = "ADMIN,MANAGER,STAFF")]
        public async Task<IActionResult> ExportMonthlyPdf(int year, int month)
        {
            try
            {
                if (month < 1 || month > 12)
                {
                    return BadRequest(new ApiResponse<object>(400, "Tháng phải từ 1 đến 12"));
                }

                if (year < 2000 || year > DateTime.Now.Year + 1)
                {
                    return BadRequest(new ApiResponse<object>(400, "Năm không hợp lệ"));
                }

                var fromDate = new DateTime(year, month, 1);
                var toDate = fromDate.AddMonths(1).AddDays(-1);

                var (_, _, pdfFile) = await _orderReportService.GetOrderReportAsync(fromDate, toDate, isExportPdf: true);

                if (pdfFile == null || pdfFile.Length == 0)
                {
                    return StatusCode(500, new ApiResponse<object>(500, "Không thể tạo file PDF"));
                }

                var fileName = $"BaoCaoDonHang_Thang{month:D2}_{year}.pdf";
                return File(pdfFile, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xuất PDF báo cáo tháng {Month}/{Year}", month, year);
                return StatusCode(500, new ApiResponse<object>(500, "Đã xảy ra lỗi không mong đợi."));
            }
        }

        [HttpGet("yearly/{year}")]
        [Authorize(Roles = "ADMIN,MANAGER,STAFF")]
        public async Task<IActionResult> GetYearlyReport(int year)
        {
            try
            {
                if (year < 2000 || year > DateTime.Now.Year + 1)
                {
                    return BadRequest(new ApiResponse<object>(400, "Năm không hợp lệ"));
                }

                var fromDate = new DateTime(year, 1, 1);
                var toDate = new DateTime(year, 12, 31);

                var (report, summary, _) = await _orderReportService.GetOrderReportAsync(fromDate, toDate);

                var response = new OrderReportResponseDto
                {
                    Report = report,
                    Summary = summary
                };

                return Ok(new ApiResponse<OrderReportResponseDto>(200, "Lấy báo cáo năm thành công", response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy báo cáo năm {Year}", year);
                return StatusCode(500, new ApiResponse<object>(500, "Đã xảy ra lỗi không mong đợi."));
            }
        }

        [HttpGet("yearly/{year}/export-pdf")]
        [Authorize(Roles = "ADMIN,MANAGER,STAFF")]

        public async Task<IActionResult> ExportYearlyPdf(int year)
        {
            try
            {
                if (year < 2000 || year > DateTime.Now.Year + 1)
                {
                    return BadRequest(new ApiResponse<object>(400, "Năm không hợp lệ"));
                }

                var fromDate = new DateTime(year, 1, 1);
                var toDate = new DateTime(year, 12, 31);

                var (_, _, pdfFile) = await _orderReportService.GetOrderReportAsync(fromDate, toDate, isExportPdf: true);

                if (pdfFile == null || pdfFile.Length == 0)
                {
                    return StatusCode(500, new ApiResponse<object>(500, "Không thể tạo file PDF"));
                }

                var fileName = $"BaoCaoDonHang_Nam{year}.pdf";
                return File(pdfFile, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xuất PDF báo cáo năm {Year}", year);
                return StatusCode(500, new ApiResponse<object>(500, "Đã xảy ra lỗi không mong đợi."));
            }
        }

        [HttpGet("today")]
        [Authorize(Roles = "ADMIN,MANAGER,STAFF")]
        public async Task<IActionResult> GetTodayReport()
        {
            try
            {
                var today = DateTime.Today;
                var (report, summary, _) = await _orderReportService.GetOrderReportAsync(today, today);

                var response = new OrderReportResponseDto
                {
                    Report = report,
                    Summary = summary
                };

                return Ok(new ApiResponse<OrderReportResponseDto>(200, "Lấy báo cáo hôm nay thành công", response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy báo cáo hôm nay");
                return StatusCode(500, new ApiResponse<object>(500, "Đã xảy ra lỗi không mong đợi."));
            }
        }

        [HttpGet("this-week")]
        [Authorize(Roles = "ADMIN,MANAGER,STAFF")]
        [ProducesResponseType(typeof(OrderReportResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetThisWeekReport()
        {
            try
            {
                var today = DateTime.Today;
                var dayOfWeek = (int)today.DayOfWeek;
                var startOfWeek = today.AddDays(-(dayOfWeek == 0 ? 6 : dayOfWeek - 1)); // Monday
                var endOfWeek = startOfWeek.AddDays(6); // Sunday

                var (report, summary, _) = await _orderReportService.GetOrderReportAsync(startOfWeek, endOfWeek);

                var response = new OrderReportResponseDto
                {
                    Report = report,
                    Summary = summary
                };

                return Ok(new ApiResponse<OrderReportResponseDto>(200, "Lấy báo cáo tuần này thành công", response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy báo cáo tuần này");
                return StatusCode(500, new ApiResponse<object>(500, "Đã xảy ra lỗi không mong đợi."));
            }
        }

        [HttpGet("this-month")]
        [Authorize(Roles = "ADMIN,MANAGER,STAFF")]
        public async Task<IActionResult> GetThisMonthReport()
        {
            try
            {
                var today = DateTime.Today;
                var startOfMonth = new DateTime(today.Year, today.Month, 1);
                var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

                var (report, summary, _) = await _orderReportService.GetOrderReportAsync(startOfMonth, endOfMonth);

                var response = new OrderReportResponseDto
                {
                    Report = report,
                    Summary = summary
                };

                return Ok(new ApiResponse<OrderReportResponseDto>(200, "Lấy báo cáo tháng này thành công", response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy báo cáo tháng này");
                return StatusCode(500, new ApiResponse<object>(500, "Đã xảy ra lỗi không mong đợi."));
            }
        }

        [HttpGet("this-year")]
        [Authorize(Roles = "ADMIN,MANAGER,STAFF")]
        public async Task<IActionResult> GetThisYearReport()
        {
            try
            {
                var today = DateTime.Today;
                var startOfYear = new DateTime(today.Year, 1, 1);
                var endOfYear = new DateTime(today.Year, 12, 31);

                var (report, summary, _) = await _orderReportService.GetOrderReportAsync(startOfYear, endOfYear);

                var response = new OrderReportResponseDto
                {
                    Report = report,
                    Summary = summary
                };

                return Ok(new ApiResponse<OrderReportResponseDto>(200, "Lấy báo cáo năm nay thành công", response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy báo cáo năm nay");
                return StatusCode(500, new ApiResponse<object>(500, "Đã xảy ra lỗi không mong đợi."));
            }
        }
    }
}