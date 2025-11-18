using Infrastructure.Core.PropertyConverters;
using Menu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Menu.Infrastructure.Persistence.EntityConfigurations
{
    public sealed class FoodConfiguration : IEntityTypeConfiguration<Food>
    {
        public void Configure(EntityTypeBuilder<Food> entity)
        {
            entity.ToTable("Food");

            entity.HasKey(f => f.Id);
            entity.Property(f => f.Id).HasColumnName("IdFood");

            entity.Property(f => f.FoodName)
                .HasConversion(MenuConverters.FoodNameConverter)
                .HasMaxLength(50)
                .IsRequired();
            entity.HasIndex(f => f.FoodName).IsUnique()
                .HasDatabaseName("IX_Food_FoodName_Unique");

            entity.Property(f => f.Img)
                .HasConversion(CommonConverterExtension.ImgConverter)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(f => f.Description)
                .HasConversion(CommonConverterExtension.DescriptionConverter)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(f => f.FoodStatus)
                .HasConversion(MenuConverters.FoodStatusConverter)
                .IsRequired();

            entity.Property(f => f.Money)
                .HasConversion(MoneyConverterExtension.MoneyConverter)
                .HasColumnType("nvarchar(max)")
                .IsRequired();

            entity.Property(f => f.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            entity.Property(f => f.UpdatedAt)
                .IsRequired();

            entity.Property(f => f.FoodTypeId)
                .IsRequired();
            entity.HasIndex(f => f.FoodTypeId);

            entity.HasOne<FoodType>()
                .WithMany()
                .HasForeignKey(f => f.FoodTypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Food_FoodType_FoodTypeId");

            // Configure FoodRecipes as owned collection
            entity.OwnsMany(f => f.FoodRecipes, recipe =>
            {
                recipe.ToTable("FoodRecipe");

                recipe.WithOwner()
                    .HasForeignKey(r => r.FoodId);

                recipe.HasKey(r => r.Id);
                recipe.Property(r => r.Id).HasColumnName("IdFoodRecipe");

                recipe.Property(r => r.FoodId)
                    .HasColumnName("FoodId")
                    .IsRequired();

                recipe.Property(r => r.IngredientsId)
                    .HasColumnName("IngredientsId")
                    .IsRequired();

                recipe.Property(r => r.Measurement)
                    .HasConversion(MeasurementConverterExtension.MeasurementConverter)
                    .HasColumnType("nvarchar(max)")
                    .IsRequired();

                recipe.HasIndex(r => new { r.FoodId, r.IngredientsId })
                    .IsUnique()
                    .HasDatabaseName("IX_FoodRecipe_FoodId_IngredientsId_Unique");

                // Index for queries by ingredient
                recipe.HasIndex(r => r.IngredientsId)
                    .HasDatabaseName("IX_FoodRecipe_IngredientsId");
            });
        }
    }
}