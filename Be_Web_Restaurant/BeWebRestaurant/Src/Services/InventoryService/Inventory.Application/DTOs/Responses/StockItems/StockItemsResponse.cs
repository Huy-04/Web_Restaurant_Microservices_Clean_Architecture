using Common.DTOs.Responses.Measurement;
using Inventory.Domain.Enums;
using Inventory.Domain.ValueObjects.StockItems;

namespace Inventory.Application.DTOs.Responses.StockItems
{
    public sealed record StockItemsResponse(
        Guid IdStockItems,
        Guid StockId,
        string StockName,
        Guid IngredientsId,
        string IngredientsName,
        MeasurementResponse Measurement,
        decimal Capacity,
        StockItemsStatusEnum StockItemsStatus,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt
        )
    {
    }
}