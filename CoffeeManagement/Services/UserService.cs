using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
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
        private readonly ICloudinaryService _cloud;

        public UserService(UserManager<ApplicationUser> userManager, ICloudinaryService cloud)
        {
            _userManager = userManager;
            _cloud = cloud;

        }

        public async Task<IReadOnlyList<UserResultDto>> Get()
        {
            var users = await _userManager.Users.Where(u => u.IsActive).ToListAsync();
            return users.Select(MapToDto).ToList();
        }

        public async Task<UserResultDto?> GetById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;
            return MapToDto(user);
        }

        public async Task<UserResultDto> Create(CreateUserRequest model)
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

            if (model.AvatarUrl != null)
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(model.AvatarUrl.FileName, model.AvatarUrl.OpenReadStream()),
                    Folder = "coffee/users"
                };
                var uploadResult = await _cloud.UploadAsync(uploadParams);
                user.AvatarUrl = uploadResult.SecureUrl.ToString();
            }

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join("; ", result.Errors.Select(e => e.Description)));
            }

            if (!string.IsNullOrEmpty(model.RoleName))
            {
                await _userManager.AddToRoleAsync(user, model.RoleName);
            }

            return new UserResultDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FullName = user.FullName,
                EmployeeCode = user.EmployeeCode,
                IsActive = user.IsActive,
                AvatarUrl = user.AvatarUrl
            };
        }

        public async Task<UserResultDto> Update(string userId, UpdateUserRequest model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new Exception("Nhân viên không tồn tại");

            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.FullName = model.FullName;
            user.EmployeeCode = model.EmployeeCode;
            user.IsActive = model.IsActive;

            if (model.AvatarUrl != null)
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(model.AvatarUrl.FileName, model.AvatarUrl.OpenReadStream()),
                    Folder = "coffee/users"
                };
                var uploadResult = await _cloud.UploadAsync(uploadParams);
                user.AvatarUrl = uploadResult.SecureUrl.ToString();
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join("; ", result.Errors.Select(e => e.Description)));
            }

            if (!string.IsNullOrEmpty(model.RoleName))
            {
                var currentRoles = await _userManager.GetRolesAsync(user);

                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, model.RoleName);
            }

            return new UserResultDto
            {
                Id = user.Id,
                UserName = user.UserName ?? "",
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FullName = user.FullName,
                EmployeeCode = user.EmployeeCode,
                IsActive = user.IsActive,
                AvatarUrl = user.AvatarUrl
            };
        }

        public async Task<bool> Delete(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            user.IsActive = false;
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> ChangePassword(string userId, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return result.Succeeded;
        }

        private UserResultDto MapToDto(ApplicationUser user)
        {
            return new UserResultDto
            {
                Id = user.Id,
                UserName = user.UserName ?? "",
                Email = user.Email ?? "",
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber ?? "",
                EmployeeCode = user.EmployeeCode,
                IsActive = user.IsActive,
                AvatarUrl = user.AvatarUrl
            };
        }

        public async Task<IReadOnlyList<UserResultDto>> GetNonAdminUsers()
        {
            var users = await _userManager.Users
                .Where(u => u.IsActive) // chỉ lấy user đang active
                .ToListAsync();

            var result = new List<UserResultDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (!roles.Contains("ADMIN"))
                {
                    result.Add(MapToDto(user));
                }
            }

            return result;
        }

    }
}
