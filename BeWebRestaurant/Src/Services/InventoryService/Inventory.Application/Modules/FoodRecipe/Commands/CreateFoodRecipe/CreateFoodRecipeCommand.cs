using Inventory.Application.DTOs.Requests.FoodRecipe;
using Inventory.Application.DTOs.Responses.FoodRecipe;
using MediatR;

namespace Inventory.Application.Modules.FoodRecipe.Commands.CreateFoodRecipe
{
    public sealed record CreateFoodRecipeCommand(FoodRecipeRequest Request) : IRequest<FoodRecipeResponse>
    {
    }
}