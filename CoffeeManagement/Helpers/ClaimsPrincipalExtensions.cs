using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CoffeeManagement.Helpers
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal user) =>
                    user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                    ?? throw new UnauthorizedAccessException("Không xác định userId");

        //public static string GetEmail(this ClaimsPrincipal user) =>
        //    user.FindFirst(JwtRegisteredClaimNames.Email)?.Value
        //    ?? throw new UnauthorizedAccessException("Không xác định email");

        //public static string GetFullName(this ClaimsPrincipal user) =>
        //    user.FindFirst("fullName")?.Value ?? "";

    }
}
