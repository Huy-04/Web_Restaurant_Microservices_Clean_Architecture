using Menu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Menu.Infrastructure.Persistence.EntityConfigurations
{
    public class FoodTypeConfiguration : IEntityTypeConfiguration<FoodType>
    {
        public void Configure(EntityTypeBuilder<FoodType> entity)
        {
            entity.ToTable("FoodType");

            entity.HasKey(ft => ft.Id);
            entity.Property(ft => ft.Id).HasColumnName("IdFoodType");

            entity.Property(ft => ft.FoodTypeName)
                .HasConversion(MenuConverters.FoodTypeNameConverter)
                .HasMaxLength(50)
                .IsRequired();

            entity.HasIndex(ft => ft.FoodTypeName).IsUnique();

            entity.Property(ft => ft.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            entity.Property(ft => ft.UpdatedAt)
                .IsRequired();

            entity.HasMany<Food>()
                  .WithOne()
                  .HasForeignKey(f => f.FoodTypeId)
                  .OnDelete(DeleteBehavior.Restrict)
                  .HasConstraintName("FK_Food_FoodType_FoodTypeId");
        }
    }
}