using Domain.Core.Interface.Event;

namespace Menu.Domain.Events.FoodRecipeEvents
{
    public class FoodRecipeDeletedEvent : IDomainEvent
    {
        public Guid IdFoodRecipe { get; }
        public DateTimeOffset UpdatedAt { get; }
        public DateTimeOffset OccurredOn { get; }

        public FoodRecipeDeletedEvent(Guid idFoodRecipe, DateTimeOffset updatedAt)
        {
            IdFoodRecipe = idFoodRecipe;
            UpdatedAt = updatedAt;
            OccurredOn = DateTimeOffset.UtcNow;
        }
    }
}
