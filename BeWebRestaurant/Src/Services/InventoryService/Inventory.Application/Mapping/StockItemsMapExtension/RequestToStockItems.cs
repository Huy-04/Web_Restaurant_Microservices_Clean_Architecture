using Domain.Core.ValueObjects;
using Inventory.Application.DTOs.Requests.StockItems;
using Inventory.Domain.Entities;
using Inventory.Domain.ValueObjects.StockItems;

namespace Inventory.Application.Mapping.StockItemsMapExtension
{
    public static class RequestToStockItems
    {
        public static StockItems ToStockItems(this CreateStockItemsRequest request)
        {
            return StockItems.Create(
                request.StockId,
                request.IngredientsId,
                Capacity.Create(request.Capacity),
                request.UnitEnum
                );
        }

        public static void ApplyStockItems(this StockItems stockItems, UpdateStockItemsRequest request)
        {
            stockItems.UpdateStockId(request.StockId);
            stockItems.UpdateIngredientsId(request.IngredientsId);
            stockItems.UpdateCapacity(Capacity.Create(request.Capacity));
            stockItems.UpdateMeasurement(Measurement.Create(request.Measurement.Quantity, request.Measurement.UnitEnum));
            stockItems.UpdateStatus(StockItemsStatus.Create(request.StockItemsStatus));
        }
    }
}