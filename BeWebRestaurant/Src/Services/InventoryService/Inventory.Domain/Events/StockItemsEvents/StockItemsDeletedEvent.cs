using Domain.Core.Interface.Event;

namespace Inventory.Domain.Events.StockItemsEvents
{
    public class StockItemsDeletedEvent : IDomainEvent
    {
        public Guid IdStockItems { get; }
        public DateTimeOffset DeletedAt { get; }
        public DateTimeOffset OccurredOn { get; }

        public StockItemsDeletedEvent(Guid idStockItems, DateTimeOffset deletedAt)
        {
            IdStockItems = idStockItems;
            DeletedAt = deletedAt;
            OccurredOn = DateTimeOffset.UtcNow;
        }
    }
}
