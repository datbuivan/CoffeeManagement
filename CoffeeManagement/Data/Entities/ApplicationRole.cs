using Microsoft.AspNetCore.Identity;

namespace CoffeeManagement.Data.Entities
{
    public class ApplicationRole : IdentityRole
    {
        public string? Description { get; set; }
    }
}
