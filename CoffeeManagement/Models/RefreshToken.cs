using CoffeeManagement.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace CoffeeManagement.Models
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Token { get; set; } = string.Empty;

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public DateTime ExpiryTime { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public string? DeviceInfo { get; set; }

        public bool IsRevoked { get; set; } = false;

        public DateTime? RevokedDate { get; set; }

        // Navigation property
        public virtual ApplicationUser User { get; set; } = null!;
    }

}
