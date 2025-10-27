using Domain.Core.Interface.Event;

namespace Inventory.Domain.Events.InventoryEvents
{
    public class FoodRecipeDeletedEvent : IDomainEvent
    {
        public Guid IdFoodRecipe { get; }
        public DateTimeOffset DeletedAt { get; }
        public DateTimeOffset OccurredOn { get; }

        public FoodRecipeDeletedEvent(Guid idFoodRecipe, DateTimeOffset deletedAt)
        {
            IdFoodRecipe = idFoodRecipe;
            DeletedAt = deletedAt;
            OccurredOn = DateTimeOffset.UtcNow;
        }
    }
}
