using Domain.Core.Interface.Event;

namespace Inventory.Domain.Events.StockEvents
{
    public class StockUpdatedEvent : IDomainEvent
    {
        public Guid IdStock { get; }
        public DateTimeOffset UpdatedAt { get; }
        public DateTimeOffset OccurredOn { get; }

        public StockUpdatedEvent(Guid idStock, DateTimeOffset updatedAt)
        {
            IdStock = idStock;
            UpdatedAt = updatedAt;
            OccurredOn = DateTimeOffset.UtcNow;
        }
    }
}
