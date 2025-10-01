using CoffeeManagement.Data.Entities;
using CoffeeManagement.Interface;
using CoffeeManagement.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CoffeeManagement.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        // private readonly IGenericRepository<ApplicationUser> _userRepository; 

        public UserService(UserManager<ApplicationUser> userManager /*, IGenericRepository<ApplicationUser> userRepository*/)
        {
            _userManager = userManager;
            // _userRepository = userRepository;
        }

        public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<IReadOnlyList<ApplicationUser>> GetEmployeesAsync()
        {
            // Sử dụng Identity's Users DbSet để lấy danh sách
            return await _userManager.Users.Where(u => u.IsActive).ToListAsync();
        }

        public async Task<IdentityResult> CreateEmployeeAsync(CreateUserRequest model)
        {
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                FullName = model.FullName,
                EmployeeCode = model.EmployeeCode,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded && model.RoleNames != null && model.RoleNames.Any())
            {
                // Gán Roles sau khi tạo user
                await _userManager.AddToRolesAsync(user, model.RoleNames);
            }

            return result;
        }

        public async Task<IdentityResult> UpdateEmployeeAsync(string userId, UpdateUserRequest model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "Nhân viên không tồn tại." });

            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.FullName = model.FullName;
            user.EmployeeCode = model.EmployeeCode;
            user.IsActive = model.IsActive;

            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> DeleteEmployeeAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return IdentityResult.Success;

            // Xóa mềm: Vô hiệu hóa tài khoản (giữ lại lịch sử giao dịch)
            user.IsActive = false;
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> ChangePasswordAsync(string userId, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "Nhân viên không tồn tại." });

            // Admin dùng token để đổi mật khẩu (bỏ qua mật khẩu cũ)
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return await _userManager.ResetPasswordAsync(user, token, newPassword);
        }
    }
}
