using CoffeeManagement.Data.Entities;
using System.Text.Json;

namespace CoffeeManagement.Data
{
    public static class DataContextSeed
    {
        public static async Task SeedAsync(DataContext context)
        {
            if(!context.Categories.Any())
            {
                var categoryData = File.ReadAllText("./Data/SeedData/Category.json");
                var category = JsonSerializer.Deserialize<List<Category>>(categoryData);
                if(category != null)
                {
                    context.Categories.AddRange(category);
                }
            }
            if (!context.Products.Any())
            {
                var productData = File.ReadAllText("./Data/SeedData/Product.json");
                var product = JsonSerializer.Deserialize<List<Product>>(productData);
                if (product != null)
                {
                    context.Products.AddRange(product);
                }
            }

            if (!context.ProductSizes.Any())
            {
                var productSizeData = File.ReadAllText("./Data/SeedData/ProductSize.json");
                var productSize = JsonSerializer.Deserialize<List<ProductSize>>(productSizeData);
                if (productSize != null)
                {
                    context.ProductSizes.AddRange(productSize);
                }
            }

            if (context.ChangeTracker.HasChanges()) await context.SaveChangesAsync();
        }
    }
}
