using CoffeeManagement.Interface;
using CoffeeManagement.Repositories;

namespace CoffeeManagement.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));
            services.AddHttpContextAccessor();
            return services;
        }
    }
}
