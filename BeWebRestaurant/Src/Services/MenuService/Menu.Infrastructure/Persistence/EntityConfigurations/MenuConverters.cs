using Menu.Domain.Enums;
using Menu.Domain.ValueObjects.Food;
using Menu.Domain.ValueObjects.FoodType;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Menu.Infrastructure.Persistence.EntityConfigurations
{
    public static class MenuConverters
    {
        // Food
        public static readonly ValueConverter<FoodName, string>
            FoodNameConverter = new(v => v.Value, v => FoodName.Create(v));

        public static readonly ValueConverter<FoodStatus, int>
            FoodStatusConverter = new(v => (int)v.Value, v => FoodStatus.Create((FoodStatusEnum)v));

        // FoodType
        public static readonly ValueConverter<FoodTypeName, string>
            FoodTypeNameConverter = new(v => v.Value, v => FoodTypeName.Create(v));
    }
}