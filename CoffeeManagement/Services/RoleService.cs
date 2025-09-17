using CoffeeManagement.Data.Entities;
using CoffeeManagement.Interface;
using Microsoft.AspNetCore.Identity;

namespace CoffeeManagement.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ILogger<RoleService> _log;

        public RoleService(
            RoleManager<ApplicationRole> roleManager,
            ILogger<RoleService> log)
        {
            _roleManager = roleManager;
            _log = log;
        }
        public async Task<List<ApplicationRole>> GetRolesAsync() =>
        _roleManager.Roles.ToList();

        public async Task<ApplicationRole?> GetRoleByIdAsync(string id) =>
            await _roleManager.FindByIdAsync(id);

        public async Task<ApplicationRole> AddRoleAsync(ApplicationRole role)
        {
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded) throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            return role;
        }

        public async Task<ApplicationRole> UpdateRoleAsync(ApplicationRole role)
        {
            var result = await _roleManager.UpdateAsync(role);
            if (!result.Succeeded) throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            return role;
        }

        public async Task DeleteRoleAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role != null) await _roleManager.DeleteAsync(role);
        }
    }
}
