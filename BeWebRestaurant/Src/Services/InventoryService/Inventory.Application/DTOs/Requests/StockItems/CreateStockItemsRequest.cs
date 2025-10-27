using Domain.Core.Enums;

namespace Inventory.Application.DTOs.Requests.StockItems
{
    public sealed record CreateStockItemsRequest(
        Guid StockId,
        Guid IngredientsId,
        decimal Capacity,
        UnitEnum UnitEnum)
    {
    }
}