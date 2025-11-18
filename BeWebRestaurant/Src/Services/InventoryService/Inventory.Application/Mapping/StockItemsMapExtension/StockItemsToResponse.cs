using Application.Core.Mapping.MeasurementMapExtension;
using Inventory.Application.DTOs.Responses.StockItems;
using Inventory.Domain.Entities;

namespace Inventory.Application.Mapping.StockItemsMapExtension
{
    public static class StockItemsToResponse
    {
        public static StockItemsResponse ToStockItemsResponse
            (this StockItems stockItems, string stockName, string ingredientsName)
        {
            return new(
                stockItems.Id,
                stockItems.StockId,
                stockName,
                stockItems.IngredientsId,
                ingredientsName,
                stockItems.Measurement.ToMeasurementResponse(),
                stockItems.Capacity,
                stockItems.StockItemsStatus.Value,
                stockItems.CreatedAt,
                stockItems.UpdatedAt);
        }
    }
}