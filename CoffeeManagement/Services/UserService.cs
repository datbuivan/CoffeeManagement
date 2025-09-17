using CoffeeManagement.Data.Entities;
using CoffeeManagement.Interface;
using Microsoft.AspNetCore.Identity;

namespace CoffeeManagement.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserService> _log;
        public UserService(
        UserManager<ApplicationUser> userManager,
        ILogger<UserService> log)
        {
            _userManager = userManager;
            _log = log;
        }

        public async Task<List<ApplicationUser>> GetUsersAsync() =>
        _userManager.Users.ToList();

        public async Task<ApplicationUser?> GetUserByIdAsync(string id) =>
            await _userManager.FindByIdAsync(id);

        public async Task<ApplicationUser> AddUserAsync(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded) throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            return user;
        }

        public async Task<ApplicationUser> UpdateUserAsync(ApplicationUser user)
        {
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            return user;
        }

        public async Task DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null) await _userManager.DeleteAsync(user);
        }


    }
}
