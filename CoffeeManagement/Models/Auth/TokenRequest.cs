namespace CoffeeManagement.Models.Auth
{
    public class TokenRequest
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}