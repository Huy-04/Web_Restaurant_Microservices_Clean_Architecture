using Application.Core.DTOs.Requests.Measurement;
using Inventory.Domain.Enums;

namespace Inventory.Application.DTOs.Requests.StockItems
{
    public sealed record UpdateStockItemsRequest(
        Guid StockId,
        Guid IngredientsId,
        MeasurementRequest Measurement,
        decimal Capacity,
        StockItemsStatusEnum StockItemsStatus)
    {
    }
}