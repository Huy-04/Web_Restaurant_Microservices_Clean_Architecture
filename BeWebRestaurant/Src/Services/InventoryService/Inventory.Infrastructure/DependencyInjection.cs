using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Inventory.Application.IUnitOfWork;
using Inventory.Domain.IRepository;
using Inventory.Infrastructure.Persistence;
using Inventory.Infrastructure.Repository;
using Inventory.Infrastructure.UnitOfWork;

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
            // DEPRECATED: Child entities no longer have separate repositories
            // services.AddScoped<IStockItemsRepository, StockItemsRepository>();
            services.AddScoped<IIngredientsRepository, IngredientsRepository>();
            // services.AddScoped<IFoodRecipesRepository, FoodRecipesRepository>();
            
            // Unit of Work
            services.AddScoped<IInventoryUnitOfWork, InventoryUnitOfWork>();

            return services;
        }
    }
}
