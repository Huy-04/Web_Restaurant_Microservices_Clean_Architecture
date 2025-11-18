using Domain.Core.Interface.Event;

namespace Inventory.Domain.Events.StockEvents
{
    public class StockDeletedEvent : IDomainEvent
    {
        public Guid IdStock { get; }
        public DateTimeOffset DeletedAt { get; }
        public DateTimeOffset OccurredOn { get; }

        public StockDeletedEvent(Guid idStock, DateTimeOffset deletedAt)
        {
            IdStock = idStock;
            DeletedAt = deletedAt;
            OccurredOn = DateTimeOffset.UtcNow;
        }
    }
}
