using CoffeeManagement.Data.Entities;
using System.Security.Claims;

namespace CoffeeManagement.Interface
{
    public interface IJwtService
    {
        // Tạo Access Token
        Task<string> GenerateAccessTokenAsync(ApplicationUser user);

        // Tạo Refresh Token (chuỗi ngẫu nhiên)
        string GenerateRefreshToken();

        // Trích xuất Principal (Claims) từ Access Token đã hết hạn
        string? GetPrincipalFromExpiredToken(string token);
    }
}
