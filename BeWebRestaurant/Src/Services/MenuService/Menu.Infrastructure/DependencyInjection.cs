using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Menu.Application.Interfaces;
using Menu.Infrastructure.Persistence;
using Menu.Infrastructure.Repository;

namespace Menu.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMenuInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MenuDbContext>(options =>
            {
                var cs = configuration.GetConnectionString("MenuDatabase");
                options.UseSqlServer(cs);
                options.EnableSensitiveDataLogging(false);
            });

            services.AddScoped<IFoodRepository, FoodRepository>();
            services.AddScoped<IFoodTypeRepository, FoodTypeRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // MediatR is already registered in API; UnitOfWork depends on IMediator for event dispatch

            return services;
        }
    }
}
