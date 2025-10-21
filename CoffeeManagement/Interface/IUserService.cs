using CoffeeManagement.Data.Entities;
using CoffeeManagement.Models.User;
using Microsoft.AspNetCore.Identity;

namespace CoffeeManagement.Interface
{
    public interface IUserService
    {
        Task<IReadOnlyList<UserResultDto>> Get();

        Task<UserResultDto?> GetById(string userId);

        Task<UserResultDto> Create(CreateUserRequest model);
        Task<UserResultDto> Update(string userId, UpdateUserRequest model);
        Task<bool> Delete(string userId);
        Task<bool> ChangePassword(string userId, string newPassword);
        Task<IReadOnlyList<UserResultDto>> GetNonAdminUsers();
    }
}
