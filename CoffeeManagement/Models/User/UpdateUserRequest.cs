using System.ComponentModel.DataAnnotations;

namespace CoffeeManagement.Models.User
{

    public class UpdateUserRequest
    {
        [Required]
        public string FullName { get; set; } = null!;

        [Required]
        public string EmployeeCode { get; set; } = null!;
        public string UserName { get; set; } = null!;


        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public bool IsActive { get; set; }

        public IFormFile? AvatarUrl { get; set; }
        public string? RoleName { get; set; }

    }
}