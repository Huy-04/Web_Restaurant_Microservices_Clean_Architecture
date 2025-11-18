using Domain.Core.Interface.Event;
using Domain.Core.ValueObjects;
using Inventory.Domain.ValueObjects.Stock;

namespace Inventory.Domain.Events.StockEvents
{
    public class StockCreatedEvent : IDomainEvent
    {
        public Guid IdStock { get; }
        public StockName StockName { get; }
        public Description Description { get; }
        public DateTimeOffset OccurredOn { get; }

        public StockCreatedEvent(Guid idStock, StockName stockName, Description description)
        {
            IdStock = idStock;
            StockName = stockName;
            Description = description;
            OccurredOn = DateTimeOffset.UtcNow;
        }
    }
}
