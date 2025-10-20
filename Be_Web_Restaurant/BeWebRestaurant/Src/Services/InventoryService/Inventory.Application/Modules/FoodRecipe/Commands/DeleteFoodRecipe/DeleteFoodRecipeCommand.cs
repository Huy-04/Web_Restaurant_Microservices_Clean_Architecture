using MediatR;

namespace Inventory.Application.Modules.FoodRecipe.Commands.DeleteFoodRecipe
{
    public sealed record DeleteFoodRecipeCommand(Guid IdFoodRecipe) : IRequest<bool>
    {
    }
}