using Domain.Core.Interface.Event;
using Inventory.Domain.Enums;

namespace Inventory.Domain.Events.StockItemsEvents
{
    public class StockItemsUpdatedEvent : IDomainEvent
    {
        public Guid IdStockItems { get; }
        public Guid StockId { get; }
        public Guid IngredientsId { get; }
        public decimal MeasurementQuantity { get; }
        public string MeasurementUnit { get; }
        public decimal CapacityValue { get; }
        public string StockItemsStatus { get; }
        public DateTimeOffset UpdatedAt { get; }
        public DateTimeOffset OccurredOn { get; }

        public StockItemsUpdatedEvent(
            Guid idStockItems,
            Guid stockId,
            Guid ingredientsId,
            decimal measurementQuantity,
            string measurementUnit,
            decimal capacityValue,
            string stockItemsStatus,
            DateTimeOffset updatedAt)
        {
            IdStockItems = idStockItems;
            StockId = stockId;
            IngredientsId = ingredientsId;
            MeasurementQuantity = measurementQuantity;
            MeasurementUnit = measurementUnit;
            CapacityValue = capacityValue;
            StockItemsStatus = stockItemsStatus;
            UpdatedAt = updatedAt;
            OccurredOn = DateTimeOffset.UtcNow;
        }
    }
}
