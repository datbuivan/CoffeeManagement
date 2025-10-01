using CoffeeManagement.Data.Entities;
using CoffeeManagement.Models.User;
using Microsoft.AspNetCore.Identity;

namespace CoffeeManagement.Interface
{
    public interface IUserService
    {
        Task<ApplicationUser?> GetUserByIdAsync(string userId);
        Task<IReadOnlyList<ApplicationUser>> GetEmployeesAsync(); // Lấy danh sách nhân viên

        // CRUD
        Task<IdentityResult> CreateEmployeeAsync(CreateUserRequest model);
        Task<IdentityResult> UpdateEmployeeAsync(string userId, UpdateUserRequest model);
        Task<IdentityResult> DeleteEmployeeAsync(string userId); // Xóa mềm (IsActive = false)

        // Actions
        Task<IdentityResult> ChangePasswordAsync(string userId, string newPassword);
    }
}
