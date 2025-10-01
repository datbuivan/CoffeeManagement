using System.ComponentModel.DataAnnotations;

namespace CoffeeManagement.Models.User
{
    public class CreateUserRequest
    {
        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        public string FullName { get; set; } = null!;

        [Required]
        public string EmployeeCode { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public IEnumerable<string>? RoleNames { get; set; } // Roles để gán ngay khi tạo
    }
}