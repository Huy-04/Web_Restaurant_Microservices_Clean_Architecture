using Domain.Core.Interface.Event;

namespace Inventory.Domain.Events.InventoryEvents
{
    public class StockItemsUpdatedEvent : IDomainEvent
    {
        public Guid IdStockItems { get; }
        public DateTimeOffset UpdatedAt { get; }
        public DateTimeOffset OccurredOn { get; }

        public StockItemsUpdatedEvent(Guid idStockItems, DateTimeOffset updatedAt)
        {
            IdStockItems = idStockItems;
            UpdatedAt = updatedAt;
            OccurredOn = DateTimeOffset.UtcNow;
        }
    }
}
