using Menu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Menu.Infrastructure.Persistence
{
    public class MenuDbContext : DbContext
    {
        public MenuDbContext(DbContextOptions<MenuDbContext> options) : base(options)
        {
        }

        public DbSet<Food> Foods => Set<Food>();
        public DbSet<FoodType> FoodTypes => Set<FoodType>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                Assembly.GetExecutingAssembly(),
                t => t.Namespace!.Contains(".EntityConfigurations"));
        }
    }
}