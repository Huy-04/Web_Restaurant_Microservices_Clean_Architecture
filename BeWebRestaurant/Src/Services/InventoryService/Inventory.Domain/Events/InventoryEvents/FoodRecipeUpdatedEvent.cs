using Domain.Core.Interface.Event;

namespace Inventory.Domain.Events.InventoryEvents
{
    public class FoodRecipeUpdatedEvent : IDomainEvent
    {
        public Guid IdFoodRecipe { get; }
        public DateTimeOffset UpdatedAt { get; }
        public DateTimeOffset OccurredOn { get; }

        public FoodRecipeUpdatedEvent(Guid idFoodRecipe, DateTimeOffset updatedAt)
        {
            IdFoodRecipe = idFoodRecipe;
            UpdatedAt = updatedAt;
            OccurredOn = DateTimeOffset.UtcNow;
        }
    }
}
