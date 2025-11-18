using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Menu.Infrastructure.Persistence;
using Menu.Domain.IRepository.Read;
using Menu.Infrastructure.Repository.Read;
using Infrastructure.Menu.Repository.Read;
using Menu.Domain.IRepository.Write;
using Menu.Infrastructure.Repository.Write;
using Microsoft.Extensions.DependencyInjection;
using Menu.Application.IUnitOfWork;
using Menu.Infrastructure.UnitOfWork;

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
            // read
            services.AddScoped<IRFoodRepository, RFoodRepository>();
            services.AddScoped<IRFoodTypeRepository, RFoodTypeRepository>();

            // write
            services.AddScoped<IWFoodRepository, WFoodRepository>();
            services.AddScoped<IWFoodTypeRepository, WFoodTypeRepository>();

            // Unit of Work
            services.AddScoped<IMenuUnitOfWork, MenuUnitOfWork>();

            return services;
        }
    }
}