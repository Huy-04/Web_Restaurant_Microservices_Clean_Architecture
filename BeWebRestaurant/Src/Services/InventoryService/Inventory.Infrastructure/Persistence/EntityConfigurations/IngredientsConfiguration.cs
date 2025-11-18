using Infrastructure.Core.PropertyConverters;
using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Persistence.EntityConfigurations
{
    public sealed class IngredientsConfiguration : IEntityTypeConfiguration<Ingredients>
    {
        public void Configure(EntityTypeBuilder<Ingredients> entity)
        {
            entity.ToTable("Ingredients");

            entity.HasKey(i => i.Id);
            entity.Property(i => i.Id).HasColumnName("IdIngredients");

            entity.Property(i => i.IngredientsName)
                .HasConversion(InventoryConverters.IngredientsNameConverter)
                .HasMaxLength(50)
                .IsRequired();
            entity.HasIndex(i => i.IngredientsName).IsUnique();

            entity.Property(i => i.Description)
                .HasConversion(CommonConverterExtension.DescriptionConverter)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(i => i.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            entity.Property(i => i.UpdatedAt)
                .IsRequired();

            entity.HasMany<StockItems>()
                  .WithOne()
                  .HasForeignKey(si => si.IngredientsId)
                  .OnDelete(DeleteBehavior.Restrict)
                  .HasConstraintName("FK_StockItems_Ingredients_IngredientsId");

            entity.HasMany<FoodRecipe>()
                  .WithOne()
                  .HasForeignKey(fr => fr.IngredientsId)
                  .OnDelete(DeleteBehavior.Restrict)
                  .HasConstraintName("FK_FoodRecipe_Ingredients_IngredientsId");
        }
    }
}