using CoffeeManagement.Data.Entities;
using CoffeeManagement.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CoffeeManagement.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public JwtService(IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<string> GenerateAccessTokenAsync(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // Lấy Secret Key và kiểm tra null
            var secretKey = _configuration["Jwt:Secret"]
                ?? throw new InvalidOperationException("JWT Secret key is missing in configuration.");
            var key = Encoding.ASCII.GetBytes(secretKey);

            // Lấy thời gian hết hạn Access Token
            if (!double.TryParse(_configuration["Jwt:AccessTokenExpiryMinutes"], out double expiryMinutes))
            {
                expiryMinutes = 15; // Mặc định 15 phút nếu cấu hình sai
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim("fullname", user.FullName ?? user.UserName!)
            };

            // Lấy Claims về Roles
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Tạo Token Descriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                // Tính thời gian hết hạn từ cấu hình
                Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            // Tạo chuỗi Refresh Token ngẫu nhiên và an toàn (64 byte)
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public string? GetPrincipalFromExpiredToken(string token)
        {
            var secretKey = _configuration["Jwt:Secret"]
                ?? throw new InvalidOperationException("JWT Secret key is missing in configuration.");

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],

                // Cực kỳ quan trọng: Bỏ qua kiểm tra thời gian hết hạn (Expires time)
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;

            try
            {
                // Thử validate và trích xuất claims.
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

                // Kiểm tra token có phải là JWT Token với thuật toán HmacSha256 không
                if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return null;
                }

                // Trích xuất tên người dùng (UserName/Name)
                return principal.FindFirst(ClaimTypes.Name)?.Value;
            }
            catch
            {
                return null;
            }
        }

    }

}
