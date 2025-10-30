using MediatR;
using Inventory.Application.DTOs.Requests.FoodRecipe;
using Inventory.Application.DTOs.Responses.FoodRecipe;

namespace Inventory.Application.Modules.FoodRecipe.Commands.UpdateFoodRecipe
{
    public sealed record UpdateFoodRecipeCommand(Guid IdFoodRecipe, FoodRecipeRequest Request) : IRequest<FoodRecipeResponse>
    {
    }
}
