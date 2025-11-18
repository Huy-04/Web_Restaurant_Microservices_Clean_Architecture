using Domain.Core.ValueObjects;
using MediatR;

namespace Menu.Application.Modules.Food.Commands.UpdateFoodRecipe
{
    public sealed record UpdateFoodRecipeCommand(
        Guid FoodId,
        Guid RecipeId,
        Measurement Measurement) : IRequest<bool>
    {
    }
}
