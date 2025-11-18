using MediatR;

namespace Menu.Application.Modules.Food.Commands.UpdateFoodRecipe
{
    public sealed record UpdateFoodRecipeIngredientCommand(
        Guid FoodId,
        Guid RecipeId,
        Guid NewIngredientsId) : IRequest<bool>
    {
    }
}
