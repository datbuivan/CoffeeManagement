using CoffeeManagement.Data.Entities;
using CoffeeManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoffeeManagement.Data
{
    public class DataContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options) { }
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProductSize> ProductSizes => Set<ProductSize>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Ingredient> Ingredients => Set<Ingredient>();
        public DbSet<ProductIngredient> ProductIngredients => Set<ProductIngredient>();
        public DbSet<InventoryTransaction> InventoryTransactions => Set<InventoryTransaction>();
        public DbSet<ReportDailyRevenue> ReportDailyRevenues => Set<ReportDailyRevenue>();
        public DbSet<ReportInventorySummary> ReportInventorySummaries => Set<ReportInventorySummary>();
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Category - Product (1-n)
            builder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.RefreshToken).HasMaxLength(500);
                entity.Property(e => e.FullName).HasMaxLength(100);

                // Index for performance
                entity.HasIndex(e => e.RefreshToken).IsUnique().HasFilter("[RefreshToken] IS NOT NULL");
            });

            builder.Entity<ProductSize>()
                .HasOne(ps => ps.Product)
                .WithMany(p => p.ProductSizes)
                .HasForeignKey(ps => ps.ProductId);

            // Product - ProductIngredient (1-n)
            builder.Entity<ProductIngredient>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.ProductIngredients)
                .HasForeignKey(pi => pi.ProductId);

            // Ingredient - ProductIngredient (1-n)
            builder.Entity<ProductIngredient>()
                .HasOne(pi => pi.Ingredient)
                .WithMany(i => i.ProductIngredients)
                .HasForeignKey(pi => pi.IngredientId);

            // Order - OrderItem (1-n)
            builder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            // Product - OrderItem (1-n)
            builder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId);

            // Ingredient - InventoryTransaction (1-n)
            builder.Entity<InventoryTransaction>()
                .HasOne(it => it.Ingredient)
                .WithMany(i => i.InventoryTransactions)
                .HasForeignKey(it => it.IngredientId);

            // ReportInventorySummary - Ingredient (1-n)
            builder.Entity<ReportInventorySummary>()
                .HasOne(r => r.Ingredient)
                .WithMany()
                .HasForeignKey(r => r.IngredientId);
        }

    }
}
