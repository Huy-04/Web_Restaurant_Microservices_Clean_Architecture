using Inventory.Application.DTOs.Responses.FoodRecipe;
using MediatR;

namespace Inventory.Application.Modules.FoodRecipe.Queries.GetAllFoodRecipe
{
    public sealed record GetAllFoodRecipeQuery() : IRequest<IEnumerable<FoodRecipeResponse>>
    {
    }
}