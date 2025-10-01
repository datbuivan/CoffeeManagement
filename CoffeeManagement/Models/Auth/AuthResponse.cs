namespace CoffeeManagement.Models.Auth
{
    public class AuthResponse
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public List<string> Roles { get; set; } = new List<string>();
        // Optional: Expiration time for client-side reference
        public DateTime AccessTokenExpires { get; set; }
    }
}