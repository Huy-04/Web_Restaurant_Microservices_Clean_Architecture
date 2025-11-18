using Domain.Core.Interface.Event;
using Domain.Core.ValueObjects;

namespace Inventory.Domain.Events.IngredientsEvents
{
    // Raised when Ingredients is updated - contains all current ingredients properties
    public class IngredientsUpdatedEvent : IDomainEvent
    {
        public Guid IdIngredients { get; }
        public string IngredientsName { get; }
        public string Description { get; }
        public DateTimeOffset UpdatedAt { get; }
        public DateTimeOffset OccurredOn { get; }

        public IngredientsUpdatedEvent(Guid idIngredients, string ingredientsName, string description, DateTimeOffset updatedAt)
        {
            IdIngredients = idIngredients;
            IngredientsName = ingredientsName;
            Description = description;
            UpdatedAt = updatedAt;
            OccurredOn = DateTimeOffset.UtcNow;
        }
    }
}
