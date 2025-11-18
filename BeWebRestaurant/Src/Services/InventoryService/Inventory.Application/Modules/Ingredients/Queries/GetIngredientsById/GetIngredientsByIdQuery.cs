using Inventory.Application.DTOs.Responses.Ingredients;
using MediatR;

namespace Inventory.Application.Modules.Ingredients.Queries.GetIngredientsById
{
    public sealed record GetIngredientsByIdQuery(Guid IdIngredients) : IRequest<IngredientsResponse>
    {
    }
}