using Domain.Core.Interface.Event;

namespace Menu.Domain.Events.FoodRecipeEvents
{
    public class FoodRecipeUpdatedEvent : IDomainEvent
    {
        public Guid IdFoodRecipe { get; }
        public Guid FoodId { get; }
        public Guid IngredientsId { get; }
        public decimal Quantity { get; }
        public DateTimeOffset UpdatedAt { get; }
        public DateTimeOffset OccurredOn { get; }

        public FoodRecipeUpdatedEvent(Guid idFoodRecipe, Guid foodId, Guid ingredientsId, decimal quantity, DateTimeOffset updatedAt)
        {
            IdFoodRecipe = idFoodRecipe;
            FoodId = foodId;
            IngredientsId = ingredientsId;
            Quantity = quantity;
            UpdatedAt = updatedAt;
            OccurredOn = DateTimeOffset.UtcNow;
        }
    }
}
