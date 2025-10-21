namespace CoffeeManagement.Models.User
{
    public class UserResultDto
    {
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string EmployeeCode { get; set; } = null!;
        public bool IsActive { get; set; }
        public string? AvatarUrl { get; set; }
        public string RoleName { get; set; } = null!;
    }
}
