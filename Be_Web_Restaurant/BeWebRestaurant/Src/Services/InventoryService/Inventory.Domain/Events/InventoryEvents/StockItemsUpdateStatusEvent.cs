using Domain.Core.Interface.Event;
using Inventory.Domain.ValueObjects.StockItems;

namespace Inventory.Domain.Events.InventoryEvents
{
    public class StockItemsUpdateStatusEvent : IDomainEvent
    {
        public Guid IdStockItems { get; }

        public StockItemsStatus StockItemsStatus { get; }

        public DateTimeOffset UpdatedAt { get; }

        public DateTimeOffset OccurredOn { get; }

        public StockItemsUpdateStatusEvent(Guid idStockItems, StockItemsStatus stockItemsStatus, DateTimeOffset updatedAt)
        {
            IdStockItems = idStockItems;
            StockItemsStatus = stockItemsStatus;
            UpdatedAt = updatedAt;
            OccurredOn = DateTimeOffset.UtcNow;
        }
    }
}