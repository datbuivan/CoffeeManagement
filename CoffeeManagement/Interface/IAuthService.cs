
using CoffeeManagement.Models.Auth;
using Microsoft.AspNetCore.Identity;

namespace CoffeeManagement.Interface
{
    public interface IAuthService
    {
        Task<AuthResponse?> LoginAsync(LoginRequest model);
        Task LogoutAsync();
        Task<AuthResponse?> RefreshTokenAsync(TokenRequest model);
    }
}
