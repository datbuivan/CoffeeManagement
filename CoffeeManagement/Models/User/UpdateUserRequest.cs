using System.ComponentModel.DataAnnotations;

namespace CoffeeManagement.Models.User
{

    public class UpdateUserRequest
    {
        [Required]
        public string FullName { get; set; } = null!;

        [Required]
        public string EmployeeCode { get; set; } = null!;

        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; }
    }
}