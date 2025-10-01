using System.ComponentModel.DataAnnotations;

namespace CoffeeManagement.Models.User
{
    public class ChangePasswordRequest
    {
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = null!;
    }
}