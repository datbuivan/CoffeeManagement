using CoffeeManagement.Data.Entities;
using System.Text.Json;

namespace CoffeeManagement.Data
{
    public static class DataContextSeed
    {
        public static async Task SeedAsunc(DataContext context)
        {
            if(!context.Roles.Any())
            {
                var roleData = File.ReadAllText("./Data/SeedData/Role.json");
                var role = JsonSerializer.Deserialize<List<ApplicationRole>>(roleData);
                if(role != null)
                {
                    context.Roles.AddRange(role);
                }
            }
            if (context.ChangeTracker.HasChanges()) await context.SaveChangesAsync();
        }
    }
}
