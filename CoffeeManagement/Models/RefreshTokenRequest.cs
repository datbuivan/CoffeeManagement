using System.ComponentModel.DataAnnotations;

namespace CoffeeManagement.Models
{
    public class RefreshTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
