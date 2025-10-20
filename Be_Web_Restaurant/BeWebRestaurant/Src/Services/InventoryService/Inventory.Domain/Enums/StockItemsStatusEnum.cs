using System.Text.Json.Serialization;

namespace Inventory.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum StockItemsStatusEnum
    {
        Available = 1,
        OutOfStock = 2,
        LowStock = 3,
        Restocking = 4
    }
}