using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Inventory.Application.Interfaces;
using Inventory.Infrastructure.Persistence;
using Inventory.Infrastructure.Repository;

namespace Inventory.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInventoryInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<InventoryDbContext>(options =>
            {
                var cs = configuration.GetConnectionString("InventoryDatabase");
                options.UseSqlServer(cs);
            });

            services.AddScoped<IStockRepository, StockRepository>();
            services.AddScoped<IStockItemsRepository, StockItemsRepository>();
            services.AddScoped<IIngredientsRepository, IngredientsRepository>();
            services.AddScoped<IFoodRecipesRepository, FoodRecipesRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // MediatR is already registered in API; UnitOfWork depends on IMediator for event dispatch

            return services;
        }
    }
}
