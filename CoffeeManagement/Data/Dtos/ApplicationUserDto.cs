namespace CoffeeManagement.Data.Dtos
{
    public class ApplicationUserDto
    {
        public string? Id { get; set; }                  // từ IdentityUser
        public string? UserName { get; set; }            // từ IdentityUser
        public string? Email { get; set; }               // từ IdentityUser
        public string? PhoneNumber { get; set; }         // từ IdentityUser
        public string? FullName { get; set; }            // custom
        public string? Address { get; set; }             // custom
        public DateTime CreatedAt { get; set; }
    }
}
