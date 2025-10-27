using Infrastructure.Core.PropertyConverters;
using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InventoryEntity = Inventory.Domain.Entities.StockItems;

namespace Inventory.Infrastructure.Persistence.EntityConfigurations
{
    public sealed class StockItemsConfiguration : IEntityTypeConfiguration<InventoryEntity>
    {
        public void Configure(EntityTypeBuilder<InventoryEntity> entity)
        {
            entity.ToTable("StockItems");

            entity.HasKey(i => i.Id);
            entity.Property(i => i.Id).HasColumnName("IdStockItems");

            entity.Property(i => i.StockId)
                .IsRequired();
            entity.HasIndex(i => i.StockId);

            entity.Property(i => i.IngredientsId)
                .IsRequired();
            entity.HasIndex(i => i.IngredientsId);

            entity.HasIndex(i => new { i.StockId, i.IngredientsId }).IsUnique();

            entity.Property(i => i.Capacity)
                .HasConversion(InventoryConverters.CapacityConverter)
                .IsRequired();

            entity.Property(i => i.Measurement)
                .HasConversion(MeasurementConverterExtension.MeasurementConverter)
                .IsRequired();

            entity.Property(i => i.StockItemsStatus)
                .HasConversion(InventoryConverters.StockItemsStatusConverter)
                .IsRequired();

            entity.Property(i => i.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            entity.Property(i => i.UpdatedAt)
                .IsRequired();

            entity.HasOne<Stock>()
                  .WithMany()
                  .HasForeignKey(i => i.StockId)
                  .OnDelete(DeleteBehavior.Restrict)
                  .HasConstraintName("FK_StockItems_Stock_StockId");

            entity.HasOne<Ingredients>()
                  .WithMany()
                  .HasForeignKey(i => i.IngredientsId)
                  .OnDelete(DeleteBehavior.Restrict)
                  .HasConstraintName("FK_StockItems_Ingredients_IngredientsId");
        }
    }
}