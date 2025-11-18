using Inventory.Application.DTOs.Responses.Ingredients;
using MediatR;

namespace Inventory.Application.Modules.Ingredients.Queries.GetAllIngredients
{
    public sealed record GetAllIngredientsQuery() : IRequest<IEnumerable<IngredientsResponse>>
    {
    }
}