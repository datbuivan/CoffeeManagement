using CoffeeManagement.Data.Entities;
using CoffeeManagement.Interface;
using Microsoft.AspNetCore.Identity;
using CoffeeManagement.Models.Auth;

namespace CoffeeManagement.Services
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtService _jwtService;

        public AuthService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IJwtService jwtService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtService = jwtService;
        }

        public async Task<AuthResponse?> LoginAsync(LoginRequest model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName)
                               ?? await _userManager.FindByEmailAsync(model.UserName);

            if (user == null || !user.IsActive)
            {
                return null; // Return null instead of SignInResult.Failed to match new signature
            }

            var result = await _signInManager.PasswordSignInAsync(
                user,
                model.Password,
                isPersistent: model.RememberMe,
                lockoutOnFailure: true);

            if (result.Succeeded)
            {
                user.LastLoginDate = DateTime.UtcNow;

                var accessToken = await _jwtService.GenerateAccessTokenAsync(user);
                var refreshToken = _jwtService.GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // Example: 7-day refresh token lifespan

                await _userManager.UpdateAsync(user);

                var userRoles = await _userManager.GetRolesAsync(user);

                return new AuthResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    UserId = user.Id,
                    FullName = user.FullName ?? user.UserName!,
                    Roles = userRoles.ToList(),
                    AccessTokenExpires = DateTime.UtcNow.AddMinutes(15) // Example: 15-minute access token lifespan
                };
            }

            // Handle LockedOut and other non-success scenarios
            if (result.IsLockedOut)
            {
                // This path should be handled by the controller using the returned null
            }

            return null;
        }

        public async Task LogoutAsync()
        {
            var user = await _userManager.GetUserAsync(_signInManager.Context.User);
            if (user != null)
            {
                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = null;
                await _userManager.UpdateAsync(user);
            }

            await _signInManager.SignOutAsync();
        }

        public async Task<AuthResponse?> RefreshTokenAsync(TokenRequest model)
        {
            var userName = _jwtService.GetPrincipalFromExpiredToken(model.AccessToken);

            if (userName == null) return null;
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null || user.RefreshToken != model.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }

            var newAccessToken = await _jwtService.GenerateAccessTokenAsync(user);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            // 5. Update and Store the new Refresh Token
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // Reset expiration
            await _userManager.UpdateAsync(user);

            // 6. Build Response
            var userRoles = await _userManager.GetRolesAsync(user);

            return new AuthResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                UserId = user.Id,
                FullName = user.FullName ?? user.UserName!,
                Roles = userRoles.ToList(),
                AccessTokenExpires = DateTime.UtcNow.AddMinutes(15)
            };
        }

    }
}