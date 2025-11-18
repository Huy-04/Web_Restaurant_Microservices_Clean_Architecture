using Inventory.Domain.Enums;
using Inventory.Domain.ValueObjects.Ingredients;
using Inventory.Domain.ValueObjects.Stock;
using Inventory.Domain.ValueObjects.StockItems;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Inventory.Infrastructure.Persistence.EntityConfigurations
{
    public static class InventoryConverters
    {
        // Ingredients
        public static readonly ValueConverter<IngredientsName, string>
            IngredientsNameConverter = new(v => v.Value, v => IngredientsName.Create(v));

        // Stock
        public static readonly ValueConverter<StockName, string>
            StockNameConverter = new(v => v.Value, v => StockName.Create(v));

        // StockItems
        public static readonly ValueConverter<Capacity, decimal>
            CapacityConverter = new(v => v.Value, v => Capacity.Create(v));

        public static readonly ValueConverter<StockItemsStatus, int>
            StockItemsStatusConverter = new(v => (int)v.Value, v => StockItemsStatus.Create((StockItemsStatusEnum)v));
    }
}