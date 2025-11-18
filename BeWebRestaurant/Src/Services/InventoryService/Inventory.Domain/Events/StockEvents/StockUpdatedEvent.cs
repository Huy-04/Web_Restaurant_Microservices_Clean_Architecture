using Domain.Core.Interface.Event;
using Domain.Core.ValueObjects;

namespace Inventory.Domain.Events.StockEvents
{
    public class StockUpdatedEvent : IDomainEvent
    {
        public Guid IdStock { get; }
        public string StockName { get; }
        public string Description { get; }
        public DateTimeOffset UpdatedAt { get; }
        public DateTimeOffset OccurredOn { get; }

        public StockUpdatedEvent(Guid idStock, string stockName, string description, DateTimeOffset updatedAt)
        {
            IdStock = idStock;
            StockName = stockName;
            Description = description;
            UpdatedAt = updatedAt;
            OccurredOn = DateTimeOffset.UtcNow;
        }
    }
}
