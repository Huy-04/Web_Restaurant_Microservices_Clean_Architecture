using MediatR;

namespace Menu.Application.Modules.Food.Commands.RemoveFoodRecipe
{
    public sealed record RemoveFoodRecipeCommand(
        Guid FoodId,
        Guid RecipeId) : IRequest<bool>
    {
    }
}
