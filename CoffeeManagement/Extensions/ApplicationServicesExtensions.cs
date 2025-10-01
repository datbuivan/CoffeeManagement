using CoffeeManagement.Data.Dtos;
using CoffeeManagement.Data.Entities;
using CoffeeManagement.Helpers;
using CoffeeManagement.Interface;
using CoffeeManagement.Repositories;
using CoffeeManagement.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CoffeeManagement.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICloudinaryService, CloudinaryService>();
            services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));
            services.AddScoped<IProductSizeRepository, ProductSizeRepository>();


            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IVnPayService, VnPayService>();
            services.AddScoped<IOrderService, OrderService>();



            //services.AddScoped<IBaseService<CategoryDto>, CategoryService>();
            //services.AddScoped<IBaseService<ProductDto>, ProductService>();
            //services.AddScoped<IBaseService<IngredientDto>, IngredientService>();
            //services.AddScoped<IBaseService<ProductIngredientDto>, ProductIngredientService>();
            //services.AddScoped<IBaseService<OrderDto>, OrderService>();
            //services.AddScoped<IBaseService<OrderItemDto>, OrderItemService>();
            //services.AddScoped<IBaseService<InventoryTransactionDto>, InventoryTransactionService>();
            //services.AddScoped<IBaseService<ReportDailyRevenueDto>, ReportDailyRevenueService>();
            //services.AddScoped<IBaseService<ReportInventorySummaryDto>, ReportInventorySummaryService>();


            services.AddHttpContextAccessor();
            return services;
        }
    }
}
