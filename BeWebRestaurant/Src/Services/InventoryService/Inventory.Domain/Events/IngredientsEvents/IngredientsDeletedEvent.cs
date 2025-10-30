using Domain.Core.Interface.Event;

namespace Inventory.Domain.Events.IngredientsEvents
{
    public class IngredientsDeletedEvent : IDomainEvent
    {
        public Guid IdIngredients { get; }
        public DateTimeOffset DeletedAt { get; }
        public DateTimeOffset OccurredOn { get; }

        public IngredientsDeletedEvent(Guid idIngredients, DateTimeOffset deletedAt)
        {
            IdIngredients = idIngredients;
            DeletedAt = deletedAt;
            OccurredOn = DateTimeOffset.UtcNow;
        }
    }
}
