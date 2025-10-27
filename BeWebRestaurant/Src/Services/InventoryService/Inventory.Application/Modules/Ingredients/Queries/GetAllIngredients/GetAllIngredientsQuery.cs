using Inventory.Application.DTOs.Responses.Ingredients;
using MediatR;

namespace Inventory.Application.Modules.Ingredients.Queries.GetAll
{
    public sealed record GetAllIngredientsQuery() : IRequest<IEnumerable<IngredientsResponse>>
    {
    }
}