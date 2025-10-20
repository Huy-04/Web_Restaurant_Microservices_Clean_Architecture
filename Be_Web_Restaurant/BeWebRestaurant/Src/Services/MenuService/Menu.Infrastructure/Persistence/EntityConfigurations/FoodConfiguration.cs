using Common.PropertyConverters;
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

            entity.Property(f => f.Img)
                .HasConversion(CommonConverters.ImgConverter)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(f => f.Description)
                .HasConversion(CommonConverters.DescriptionConverter)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(f => f.FoodStatus)
                .HasConversion(MenuConverters.FoodStatusConverter)
                .IsRequired();

            entity.Property(f => f.Money)
                .HasConversion(CommonConverters.MoneyConverter)
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
        }
    }
}