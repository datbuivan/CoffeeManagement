namespace CoffeeManagement.Data.Dtos
{
    public class ApplicationRoleDto
    {
        public string? Id { get; set; }          // từ IdentityRole
        public string? Name { get; set; }        // từ IdentityRole
        public string? NormalizedName { get; set; } // từ IdentityRole
        public string? ConcurrencyStamp { get; set; } // từ IdentityRole
        public string? Description { get; set; } // custom field
    }
}
