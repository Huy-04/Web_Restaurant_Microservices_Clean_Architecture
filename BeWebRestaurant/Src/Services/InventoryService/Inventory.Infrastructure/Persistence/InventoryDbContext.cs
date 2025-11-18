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

        // StockItems is now an owned entity within Stock aggregate, no separate DbSet needed
        // public DbSet<StockItems> StockItems => Set<StockItems>();
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