using CoffeeManagement.Models;

namespace CoffeeManagement.Interface
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request);
        Task<bool> RevokeTokenAsync(string userId);
        Task<int> CleanupExpiredTokensAsync();
        Task<UserInfo?> GetUserProfileAsync(string userId);
    }
}
