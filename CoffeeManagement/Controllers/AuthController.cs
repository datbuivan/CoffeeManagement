using CoffeeManagement.Data.Entities;
using CoffeeManagement.Errors;
using CoffeeManagement.Helpers;
using CoffeeManagement.Interface;
using CoffeeManagement.Models;
using CoffeeManagement.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(IAuthService authService, UserManager<ApplicationUser> userManager)
        {
            _authService = authService;
            _userManager = userManager;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<AuthResponse>>> Login([FromBody] LoginRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<string>(400, "Dữ liệu không hợp lệ"));

            try
            {
                var authResponse = await _authService.LoginAsync(model);

                if (authResponse != null)
                    return Ok(new ApiResponse<AuthResponse>(200, "Đăng nhập thành công", authResponse));

                var user = await _userManager.FindByNameAsync(model.UserName)
                           ?? await _userManager.FindByEmailAsync(model.UserName);

                if (user != null)
                {
                    if (!user.IsActive)
                        return Unauthorized(new ApiResponse<string>(401, "Tài khoản không hoạt động"));

                    if (await _userManager.IsLockedOutAsync(user))
                        return StatusCode(423, new ApiResponse<string>(423, "Tài khoản bị khóa tạm thời"));
                }

                return Unauthorized(new ApiResponse<string>(401, "Tên đăng nhập hoặc mật khẩu không đúng"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(500, $"Lỗi hệ thống: {ex.Message}"));
            }
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<string>>> Logout()
        {
            try
            {
                await _authService.LogoutAsync();
                return Ok(new ApiResponse<string>(200, "Đăng xuất thành công. Refresh Token đã bị hủy."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(500, $"Lỗi hệ thống: {ex.Message}"));
            }
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<AuthResponse>>> RefreshToken([FromBody] TokenRequest model)
        {
            try
            {
                var authResponse = await _authService.RefreshTokenAsync(model);

                if (authResponse == null)
                    return Unauthorized(new ApiResponse<string>(401, "Token không hợp lệ hoặc đã hết hạn. Vui lòng đăng nhập lại"));

                return Ok(new ApiResponse<AuthResponse>(200, "Lấy token mới thành công", authResponse));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(500, $"Lỗi hệ thống: {ex.Message}"));
            }
        }
    }
}
