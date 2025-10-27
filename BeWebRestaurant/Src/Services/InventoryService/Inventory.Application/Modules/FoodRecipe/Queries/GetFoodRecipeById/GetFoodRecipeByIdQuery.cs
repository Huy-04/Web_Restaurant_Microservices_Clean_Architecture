using Inventory.Application.DTOs.Responses.FoodRecipe;
using MediatR;

namespace Inventory.Application.Modules.FoodRecipe.Queries.GetFoodRecipeById
{
    public sealed record GetFoodRecipeByIdQuery(Guid IdFoodRecipe) : IRequest<FoodRecipeResponse>
    {
    }
}