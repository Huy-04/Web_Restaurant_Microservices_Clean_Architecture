using Domain.Core.Interface.Event;

namespace Inventory.Domain.Events.StockItemsEvents
{
    public class StockItemRemovedEvent : IDomainEvent
    {
        public Guid StockId { get; }
        public Guid StockItemId { get; }
        public DateTimeOffset OccurredOn { get; }

        public StockItemRemovedEvent(Guid stockId, Guid stockItemId)
        {
            StockId = stockId;
            StockItemId = stockItemId;
            OccurredOn = DateTimeOffset.UtcNow;
        }
    }
}
