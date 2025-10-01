using CoffeeManagement.Data.Entities;
using CoffeeManagement.Helpers;
using CoffeeManagement.Interface;
using CoffeeManagement.Models;
using CoffeeManagement.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        // Inject UserManager to handle Lockout results gracefully
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(IAuthService authService, UserManager<ApplicationUser> userManager)
        {
            _authService = authService;
            _userManager = userManager;
        }


        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var authResponse = await _authService.LoginAsync(model);

            if (authResponse != null)
            {
                return Ok(authResponse);
            }

            var user = await _userManager.FindByNameAsync(model.UserName)
                               ?? await _userManager.FindByEmailAsync(model.UserName);

            if (user != null)
            {
                if (!user.IsActive)
                {
                    return Unauthorized(new { Message = "Tài khoản không hoạt động." });
                }

                var result = await _userManager.IsLockedOutAsync(user);
                if (result)
                {
                    return StatusCode(423, new { Message = "Tài khoản bị khóa tạm thời." });
                }
            }

            return Unauthorized(new { Message = "Tên đăng nhập hoặc mật khẩu không đúng." });
        }

        [HttpPost("logout")]
        [Authorize] // Requires a valid Access Token to perform this action
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return Ok(new { Message = "Đăng xuất thành công. Refresh Token đã bị hủy." });
        }


        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest model)
        {
            var authResponse = await _authService.RefreshTokenAsync(model);

            if (authResponse == null)
            {
                // Invalidate all tokens and force re-login if refresh fails
                return Unauthorized(new { Message = "Token không hợp lệ hoặc đã hết hạn. Vui lòng đăng nhập lại." });
            }

            return Ok(authResponse);
        }
    }
}
