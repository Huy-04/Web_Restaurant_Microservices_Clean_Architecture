using Domain.Core.Interface.Event;

namespace Menu.Domain.Events.FoodRecipeEvents
{
    public class FoodRecipeCreatedEvent : IDomainEvent
    {
        public Guid IdFoodRecipe { get; }
        public Guid FoodId { get; }
        public Guid IngredientsId { get; }
        public DateTimeOffset OccurredOn { get; }

        public FoodRecipeCreatedEvent(Guid idFoodRecipe, Guid foodId, Guid ingredientsId)
        {
            IdFoodRecipe = idFoodRecipe;
            FoodId = foodId;
            IngredientsId = ingredientsId;
            OccurredOn = DateTimeOffset.UtcNow;
        }
    }
}
