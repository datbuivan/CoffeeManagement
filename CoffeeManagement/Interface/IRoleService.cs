using CoffeeManagement.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace CoffeeManagement.Interface
{
    public interface IRoleService
    {
        Task<IEnumerable<ApplicationRole>> GetAllRolesAsync();
        Task<ApplicationRole?> GetRoleByIdAsync(string roleId);
        Task<IdentityResult> CreateRoleAsync(string roleName, string? description);
        Task<IdentityResult> UpdateRoleAsync(string roleId, string newName, string newDescription);
        Task<IdentityResult> DeleteRoleAsync(string roleId);

        Task<IdentityResult> UpdateUserRolesAsync(string userId, IEnumerable<string> newRoles);
    }
}
