using CoffeeManagement.Data.Entities;

namespace CoffeeManagement.Interface
{
    public interface IUserService
    {
        Task<List<ApplicationUser>> GetUsersAsync();
        Task<ApplicationUser?> GetUserByIdAsync(string id);
        Task<ApplicationUser> AddUserAsync(ApplicationUser user, string password);
        Task<ApplicationUser> UpdateUserAsync(ApplicationUser user);
        Task DeleteUserAsync(string id);
    }
}
