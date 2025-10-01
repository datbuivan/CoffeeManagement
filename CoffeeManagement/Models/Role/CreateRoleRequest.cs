using System.ComponentModel.DataAnnotations;

namespace CoffeeManagement.Models.Role
{
    public class CreateRoleRequest
    {
        [Required]
        public string RoleName { get; set; } = null!;
        public string? Description { get; set; }
    }

    public class UpdateRoleRequest
    {
        [Required]
        public string NewRoleName { get; set; } = null!;
        public string? NewDescription { get; set; }
    }

    public class AssignRolesRequest
    {
        public IEnumerable<string>? RoleNames { get; set; }
    }
}
