using CoffeeManagement.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace CoffeeManagement.Data
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            if (!roleManager.Roles.Any())
            {
                var roles = new List<ApplicationRole>
        {
            new() { Name = "ADMIN", NormalizedName = "ADMIN", Description = "Administrator with full access" },
            new() { Name = "MANAGER", NormalizedName = "MANAGER", Description = "Manager with limited access" },
            new() { Name = "STAFF", NormalizedName = "STAFF", Description = "Staff with minimal access" }
        };

                foreach (var role in roles)
                {
                    var existingRole = await roleManager.FindByNameAsync(role.Name!);
                    if (existingRole == null)
                    {
                        await roleManager.CreateAsync(role);
                    }
                    else
                    {
                        existingRole.Description = role.Description;
                        await roleManager.UpdateAsync(existingRole);
                    }
                }
            }
        }

        public static async Task SeedUsersAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var users = new List<(ApplicationUser user, string role, string password)>
    {
        (new ApplicationUser {
            UserName = "admin",
            Email = "admin@coffee.com",
            FullName = "System Admin",
            EmailConfirmed = true,
            EmployeeCode = "SYS001"
        }, "ADMIN", "Admin@123"),

        (new ApplicationUser {
            UserName = "manager",
            Email = "manager@coffee.com",
            FullName = "Store Manager",
            EmailConfirmed = true,
            EmployeeCode = "MNG002"
        }, "MANAGER", "Manager@123"),

        (new ApplicationUser {
            UserName = "staff",
            Email = "staff@coffee.com",
            FullName = "Store Staff",
            EmailConfirmed = true,
            EmployeeCode = "STF003"
        }, "STAFF", "Staff@123")
    };

            foreach (var (user, role, password) in users)
            {
                var existingUser = await userManager.FindByEmailAsync(user.Email!);
                if (existingUser == null)
                {
                    var createResult = await userManager.CreateAsync(user, password);
                    if (!createResult.Succeeded)
                    {
                        var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                        throw new Exception($"Cannot create user {user.Email}: {errors}");
                    }

                    var addRoleResult = await userManager.AddToRoleAsync(user, role);
                    if (!addRoleResult.Succeeded)
                    {
                        var errors = string.Join(", ", addRoleResult.Errors.Select(e => e.Description));
                        throw new Exception($"Cannot add user {user.Email} to role {role}: {errors}");
                    }
                }
            }
        }


    }
}
