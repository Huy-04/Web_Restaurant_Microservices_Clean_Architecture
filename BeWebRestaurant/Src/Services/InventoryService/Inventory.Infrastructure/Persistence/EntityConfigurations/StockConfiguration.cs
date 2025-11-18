using Infrastructure.Core.PropertyConverters;
using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Persistence.EntityConfigurations
{
    public sealed class StockConfiguration : IEntityTypeConfiguration<Stock>
    {
        public void Configure(EntityTypeBuilder<Stock> entity)
        {
            entity.ToTable("Stock");

            entity.HasKey(s => s.Id);
            entity.Property(s => s.Id).HasColumnName("IdStock");

            entity.Property(s => s.StockName)
                .HasConversion(InventoryConverters.StockNameConverter)
                .HasMaxLength(50)
                .IsRequired();
            entity.HasIndex(s => s.StockName).IsUnique();

            entity.Property(s => s.Description)
                .HasConversion(CommonConverterExtension.DescriptionConverter)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(s => s.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            entity.Property(s => s.UpdatedAt)
                .IsRequired();

            // Configure StockItems as owned collection
            entity.OwnsMany(s => s.StockItems, item =>
            {
                item.ToTable("StockItems");

                item.WithOwner()
                    .HasForeignKey(i => i.StockId); // Map to explicit StockId property

                item.HasKey(i => i.Id);
                item.Property(i => i.Id).HasColumnName("IdStockItems");

                item.Property(i => i.StockId)
                    .HasColumnName("StockId")
                    .IsRequired();

                item.Property(i => i.IngredientsId)
                    .HasColumnName("IngredientsId")
                    .IsRequired();

                item.Property(i => i.Measurement)
                    .HasConversion(MeasurementConverterExtension.MeasurementConverter)
                    .HasColumnType("nvarchar(max)")
                    .IsRequired();

                item.Property(i => i.Capacity)
                    .HasConversion(InventoryConverters.CapacityConverter)
                    .IsRequired();

                item.Property(i => i.StockItemsStatus)
                    .HasConversion(InventoryConverters.StockItemsStatusConverter)
                    .IsRequired();

                // Unique constraint: one stock cannot have the same ingredient twice
                item.HasIndex(i => new { i.StockId, i.IngredientsId })
                    .IsUnique()
                    .HasDatabaseName("IX_StockItems_StockId_IngredientsId_Unique");

                // Index for queries by ingredient
                item.HasIndex(i => i.IngredientsId)
                    .HasDatabaseName("IX_StockItems_IngredientsId");
            });
        }
    }
}