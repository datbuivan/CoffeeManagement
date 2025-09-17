using CoffeeManagement.Interface;
using CoffeeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.RegisterAsync(request);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.LoginAsync(request);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthResponse>> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.RefreshTokenAsync(request);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("revoke-token")]
        [Authorize]
        public async Task<ActionResult> RevokeToken([FromBody] RefreshTokenRequest request)
        {
            var result = await _authService.RevokeTokenAsync(request.RefreshToken);

            if (!result)
            {
                return BadRequest(new { message = "Token không hợp lệ" });
            }

            return Ok(new { message = "Token đã được thu hồi" });
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult> Logout()
        {
            var userId = User.FindFirst("sub")?.Value ?? User.FindFirst("nameid")?.Value;

            if (string.IsNullOrEmpty(userId))
                return BadRequest(new { message = "Không tìm thấy thông tin người dùng" });

            var result = await _authService.RevokeTokenAsync(userId);

            return result
                ? Ok(new { message = "Đăng xuất thành công" })
                : BadRequest(new { message = "Có lỗi xảy ra khi đăng xuất" });
        }

        [HttpGet("profile")]
        [Authorize]
        public ActionResult GetProfile()
        {
            var userId = User.FindFirst("sub")?.Value ?? User.FindFirst("nameid")?.Value;
            var email = User.FindFirst("email")?.Value;
            var fullName = User.FindFirst("fullName")?.Value;

            return Ok(new
            {
                id = userId,
                email = email,
                fullName = fullName
            });
        }
    }
}
