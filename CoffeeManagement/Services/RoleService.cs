using CoffeeManagement.Data.Entities;
using CoffeeManagement.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CoffeeManagement.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleService(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IEnumerable<ApplicationRole>> GetAllRolesAsync()
        {
            return await _roleManager.Roles.AsNoTracking().ToListAsync();
        }

        public async Task<ApplicationRole?> GetRoleByIdAsync(string roleId)
        {
            return await _roleManager.FindByIdAsync(roleId);
        }

        public async Task<IdentityResult> CreateRoleAsync(string roleName, string? description)
        {
            var role = new ApplicationRole { Name = roleName, Description = description };
            return await _roleManager.CreateAsync(role);
        }

        public async Task<IdentityResult> UpdateRoleAsync(string roleId, string newName, string newDescription)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                return IdentityResult.Failed(new IdentityError { Description = "Quyền không tồn tại." });

            role.Name = newName;
            role.NormalizedName = _roleManager.NormalizeKey(newName);
            role.Description = newDescription;

            return await _roleManager.UpdateAsync(role);
        }

        public async Task<IdentityResult> DeleteRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return IdentityResult.Success;

            return await _roleManager.DeleteAsync(role);
        }

        public async Task<IdentityResult> UpdateUserRolesAsync(string userId, IEnumerable<string> newRoles)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "Nhân viên không tồn tại." });

            var currentRoles = await _userManager.GetRolesAsync(user);

            // 1. Loại bỏ các roles cũ không còn
            var rolesToRemove = currentRoles.Except(newRoles);
            var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            if (!removeResult.Succeeded) return removeResult;

            // 2. Thêm các roles mới
            var rolesToAdd = newRoles.Except(currentRoles);
            return await _userManager.AddToRolesAsync(user, rolesToAdd);
        }
    }
}
