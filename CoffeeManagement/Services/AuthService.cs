using CoffeeManagement.Data;
using CoffeeManagement.Data.Entities;
using CoffeeManagement.Interface;
using CoffeeManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CoffeeManagement.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtService jwtService,
            ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    return CreateErrorResponse("Email đã được sử dụng");
                }

                var user = new ApplicationUser
                {
                    UserName = request.Email,
                    Email = request.Email,
                    FullName = request.FullName,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return CreateErrorResponse(errors);
                }

                return await CreateTokenResponseAsync(user,"Đăng ký thành công");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration");
                return CreateErrorResponse("Có lỗi xảy ra khi đăng ký");
            }
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    return CreateErrorResponse("Email hoặc mật khẩu không chính xác");
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);

                if (!result.Succeeded)
                {
                    return CreateErrorResponse("Email hoặc mật khẩu không chính xác");
                }
                return await CreateTokenResponseAsync(user, "Đăng nhập thành công");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                return CreateErrorResponse("Có lỗi xảy ra khi đăng nhập");
            }
        }

        public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            try
            {
                var user = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken);

                if (user == null)
                {
                    return CreateErrorResponse("Refresh token không hợp lệ");
                }

                if (!user.HasValidRefreshToken())
                {
                    // Clear expired token
                    user.RefreshToken = null;
                    user.RefreshTokenExpiryTime = null;
                    await _userManager.UpdateAsync(user);

                    return CreateErrorResponse("Refresh token đã hết hạn");
                }

                return await CreateTokenResponseAsync(user, "Token đã được làm mới");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during token refresh");
                return CreateErrorResponse("Có lỗi xảy ra khi làm mới token");
            }
        }
        public async Task<bool> RevokeTokenAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return false;
                }

                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = null;

                var result = await _userManager.UpdateAsync(user);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error revoking token for user: {UserId}", userId);
                return false;
            }
        }


        public async Task<int> CleanupExpiredTokensAsync()
        {
            try
            {
                var usersWithExpiredTokens = await _userManager.Users
                    .Where(u => u.RefreshToken != null &&
                               u.RefreshTokenExpiryTime.HasValue &&
                               u.RefreshTokenExpiryTime.Value <= DateTime.UtcNow)
                    .ToListAsync();

                var count = 0;
                foreach (var user in usersWithExpiredTokens)
                {
                    user.RefreshToken = null;
                    user.RefreshTokenExpiryTime = null;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        count++;
                    }
                }

                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cleaning up expired tokens");
                return 0;
            }
        }

        public async Task<UserInfo?> GetUserProfileAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) return null;

                return new UserInfo
                {
                    Id = user.Id,
                    Email = user.Email ?? string.Empty,
                    FullName = user.FullName,
                    LastLoginDate = user.LastLoginDate,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user profile");
                return null;
            }
        }

        private async Task<AuthResponse> CreateTokenResponseAsync(ApplicationUser user, string message)
        {
            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();
            var accessTokenExpiry = _jwtService.GetAccessTokenExpiry();
            var refreshTokenExpiry = _jwtService.GetRefreshTokenExpiry();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = refreshTokenExpiry;
            user.LastLoginDate = DateTime.UtcNow;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                throw new InvalidOperationException("Failed to save refresh token");
            }

            return new AuthResponse
            {
                Success = true,
                Message = message,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiry = accessTokenExpiry,
                RefreshTokenExpiry = refreshTokenExpiry,
                User = new UserInfo
                {
                    Id = user.Id,
                    Email = user.Email ?? string.Empty,
                    FullName = user.FullName,
                    LastLoginDate = user.LastLoginDate,
                }
            };
        }


        private static AuthResponse CreateErrorResponse(string message)
        {
            return new AuthResponse
            {
                Success = false,
                Message = message
            };
        }

    }
}
