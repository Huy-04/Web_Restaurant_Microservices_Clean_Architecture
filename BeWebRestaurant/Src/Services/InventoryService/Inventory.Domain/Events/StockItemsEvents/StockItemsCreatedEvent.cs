using Domain.Core.Interface.Event;

namespace Inventory.Domain.Events.StockItemsEvents
{
    public class StockItemsCreatedEvent : IDomainEvent
    {
        public Guid IdStockItems { get; }
        public Guid StockId { get; }
        public Guid IngredientsId { get; }
        public DateTimeOffset OccurredOn { get; }

        public StockItemsCreatedEvent(Guid idStockItems, Guid stockId, Guid ingredientsId)
        {
            IdStockItems = idStockItems;
            StockId = stockId;
            IngredientsId = ingredientsId;
            OccurredOn = DateTimeOffset.UtcNow;
        }
    }
}
