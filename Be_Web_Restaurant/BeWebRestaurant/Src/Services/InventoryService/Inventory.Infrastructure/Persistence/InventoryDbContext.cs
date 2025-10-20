using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Inventory.Infrastructure.Persistence
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
        {
        }

        public DbSet<FoodRecipe> FoodRecipes => Set<FoodRecipe>();
        public DbSet<StockItems> StockItems => Set<StockItems>();
        public DbSet<Ingredients> Ingredients => Set<Ingredients>();
        public DbSet<Stock> Stocks => Set<Stock>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                Assembly.GetExecutingAssembly(),
                t => t.Namespace!.Contains(".EntityConfigurations"));
        }
    }
}