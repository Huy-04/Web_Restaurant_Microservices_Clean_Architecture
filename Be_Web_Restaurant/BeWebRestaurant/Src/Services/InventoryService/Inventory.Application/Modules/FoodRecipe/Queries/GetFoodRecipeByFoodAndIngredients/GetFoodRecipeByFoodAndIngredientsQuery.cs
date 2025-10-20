using Inventory.Application.DTOs.Responses.FoodRecipe;
using MediatR;

namespace Inventory.Application.Modules.FoodRecipe.Queries.GetFoodRecipeByFoodAndIngredients
{
    public sealed record GetFoodRecipeByFoodAndIngredientsQuery(Guid FoodId, Guid IngredientsId) : IRequest<IEnumerable<FoodRecipeResponse>>
    {
    }
}