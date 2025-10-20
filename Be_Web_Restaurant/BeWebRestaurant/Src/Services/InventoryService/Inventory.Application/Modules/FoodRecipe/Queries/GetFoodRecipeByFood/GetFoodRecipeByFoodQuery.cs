using Inventory.Application.DTOs.Responses.FoodRecipe;
using MediatR;

namespace Inventory.Application.Modules.FoodRecipe.Queries.GetFoodRecipeByFood
{
    public sealed record GetFoodRecipeByFoodQuery(Guid FoodId) : IRequest<IEnumerable<FoodRecipeResponse>>
    {
    }
}