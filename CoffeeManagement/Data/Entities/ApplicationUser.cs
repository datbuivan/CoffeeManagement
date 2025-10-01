using CoffeeManagement.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CoffeeManagement.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = null!;
        public string EmployeeCode { get; set; } = null!; // Mã nhân viên
        public bool IsActive { get; set; } = true; // Trạng thái làm việc

        [StringLength(500)]
        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool HasValidRefreshToken()
        {
            return !string.IsNullOrEmpty(RefreshToken) &&
                   RefreshTokenExpiryTime.HasValue &&
                   RefreshTokenExpiryTime.Value > DateTime.UtcNow;
        }
    }
}
