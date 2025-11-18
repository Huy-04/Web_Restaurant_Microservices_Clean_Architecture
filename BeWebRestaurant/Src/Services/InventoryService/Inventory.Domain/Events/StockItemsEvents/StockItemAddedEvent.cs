using Domain.Core.Interface.Event;

namespace Inventory.Domain.Events.StockItemsEvents
{
    public class StockItemAddedEvent : IDomainEvent
    {
        public Guid StockId { get; }
        public Guid StockItemId { get; }
        public Guid IngredientsId { get; }
        public DateTimeOffset OccurredOn { get; }

        public StockItemAddedEvent(Guid stockId, Guid stockItemId, Guid ingredientsId)
        {
            StockId = stockId;
            StockItemId = stockItemId;
            IngredientsId = ingredientsId;
            OccurredOn = DateTimeOffset.UtcNow;
        }
    }
}
