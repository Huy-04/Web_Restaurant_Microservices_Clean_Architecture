using Domain.Core.Interface.Event;

namespace Inventory.Domain.Events.InventoryEvents
{
    public class IngredientsUpdatedEvent : IDomainEvent
    {
        public Guid IdIngredients { get; }
        public DateTimeOffset UpdatedAt { get; }
        public DateTimeOffset OccurredOn { get; }

        public IngredientsUpdatedEvent(Guid idIngredients, DateTimeOffset updatedAt)
        {
            IdIngredients = idIngredients;
            UpdatedAt = updatedAt;
            OccurredOn = DateTimeOffset.UtcNow;
        }
    }
}
