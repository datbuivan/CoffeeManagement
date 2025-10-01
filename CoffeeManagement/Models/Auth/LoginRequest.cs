using System.ComponentModel.DataAnnotations;
namespace CoffeeManagement.Models.Auth
{
    public class LoginRequest
    {
        [Required]
        public string UserName { get; set; } = null!; // Có thể là EmployeeCode hoặc Email

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        public bool RememberMe { get; set; }
    }
}
