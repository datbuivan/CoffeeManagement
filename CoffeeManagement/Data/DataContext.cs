using CoffeeManagement.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoffeeManagement.Data
{
    public class DataContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        // Constructor
        public DataContext(DbContextOptions<DataContext> options)
            : base(options) { }

        // DbSet Definitions (ĐÃ SỬA CHÍNH TẢ Table)
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProductSize> ProductSizes => Set<ProductSize>();
        public DbSet<Table> Tables => Set<Table>(); // 💡 Đã sửa Tabels thành Tables
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Ingredient> Ingredients => Set<Ingredient>();
        public DbSet<InventoryTransaction> InventoryTransactions => Set<InventoryTransaction>();
        public DbSet<Supplier> Suppliers => Set<Supplier>(); // 💡 Bổ sung Supplier
        public DbSet<ReportDailyRevenue> ReportDailyRevenues => Set<ReportDailyRevenue>();
        public DbSet<ReportInventorySummary> ReportInventorySummaries => Set<ReportInventorySummary>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // BẮT BUỘC: Gọi base.OnModelCreating để Identity hoạt động
            base.OnModelCreating(builder);

            // =========================================================
            // A. Cấu hình Identity (ApplicationUser/Role)
            // =========================================================
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.RefreshToken).HasMaxLength(500);
                entity.Property(e => e.FullName).HasMaxLength(100);
                entity.Property(e => e.EmployeeCode).HasMaxLength(20);

                // Index for performance
                entity.HasIndex(e => e.RefreshToken).IsUnique().HasFilter("[RefreshToken] IS NOT NULL");
                entity.HasIndex(e => e.EmployeeCode).IsUnique();
            });

            // =========================================================
            // B. Cấu hình Quan hệ (Relationships)
            // =========================================================

            // Category - Product (1-n)
            builder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            // Product - ProductSize (1-n)
            builder.Entity<ProductSize>()
                .HasOne(ps => ps.Product)
                .WithMany(p => p.ProductSizes)
                .HasForeignKey(ps => ps.ProductId);

            // Order - Table (n-1)
            builder.Entity<Order>()
                .HasOne(o => o.Table)
                .WithMany(t => t.Orders)
                .HasForeignKey(o => o.TableId)
                .IsRequired(false); // Đơn hàng có thể là Take-Away (không có bàn)

            // Order - ApplicationUser (n-1) 💡 Bổ sung liên kết nhân viên
            builder.Entity<Order>()
                .HasOne<ApplicationUser>() // Không cần Navigation Property trên Order, chỉ cần Khóa ngoại
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .IsRequired();

            // OrderItem - Order (n-1)
            builder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            // OrderItem - Product (n-1)
            builder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId);

            // OrderItem - ProductSize (n-1) 💡 Bổ sung liên kết kích cỡ
            builder.Entity<OrderItem>()
                .HasOne(oi => oi.ProductSize)
                .WithMany() // ProductSize không cần Navigation Property ngược về OrderItem
                .HasForeignKey(oi => oi.ProductSizeId)
                .IsRequired(false);

            // Ingredient - InventoryTransaction (1-n)
            builder.Entity<InventoryTransaction>()
                .HasOne(it => it.Ingredient)
                .WithMany(i => i.InventoryTransactions)
                .HasForeignKey(it => it.IngredientId);

            // InventoryTransaction - ApplicationUser (n-1) 💡 Bổ sung liên kết nhân viên thực hiện
            builder.Entity<InventoryTransaction>()
                .HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(it => it.UserId)
                .IsRequired();

            // ReportInventorySummary - Ingredient (n-1)
            builder.Entity<ReportInventorySummary>()
                .HasOne(r => r.Ingredient)
                .WithMany()
                .HasForeignKey(r => r.IngredientId);

            builder.Entity<Order>().Property(o => o.TotalAmount).HasPrecision(18, 4);
            builder.Entity<Order>().Property(o => o.DiscountAmount).HasPrecision(18, 4);
            builder.Entity<Order>().Property(o => o.FinalAmount).HasPrecision(18, 4);

            builder.Entity<OrderItem>().Property(oi => oi.UnitPrice).HasPrecision(18, 4);
            builder.Entity<OrderItem>().Property(oi => oi.SubTotal).HasPrecision(18, 4);

            builder.Entity<ProductSize>().Property(ps => ps.Price).HasPrecision(18, 4);

            builder.Entity<Ingredient>().Property(i => i.CurrentStock).HasPrecision(18, 4);
            builder.Entity<Ingredient>().Property(i => i.ReorderLevel).HasPrecision(18, 4);

            builder.Entity<InventoryTransaction>().Property(it => it.Quantity).HasPrecision(18, 4);
            builder.Entity<InventoryTransaction>().Property(it => it.UnitPrice).HasPrecision(18, 4);

            builder.Entity<ReportDailyRevenue>().Property(r => r.TotalRevenue).HasPrecision(18, 4);

            builder.Entity<ReportInventorySummary>().Property(r => r.OpeningStock).HasPrecision(18, 4);
            builder.Entity<ReportInventorySummary>().Property(r => r.InQuantity).HasPrecision(18, 4);
            builder.Entity<ReportInventorySummary>().Property(r => r.OutQuantity).HasPrecision(18, 4);
            builder.Entity<ReportInventorySummary>().Property(r => r.ClosingStock).HasPrecision(18, 4);
        }
    }
}