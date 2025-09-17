using CoffeeManagement.Data.Entities;

namespace CoffeeManagement.Interface
{
    public interface IRoleService
    {
        Task<List<ApplicationRole>> GetRolesAsync();
        Task<ApplicationRole?> GetRoleByIdAsync(string id);
        Task<ApplicationRole> AddRoleAsync(ApplicationRole role);
        Task<ApplicationRole> UpdateRoleAsync(ApplicationRole role);
        Task DeleteRoleAsync(string id);
    }
}
