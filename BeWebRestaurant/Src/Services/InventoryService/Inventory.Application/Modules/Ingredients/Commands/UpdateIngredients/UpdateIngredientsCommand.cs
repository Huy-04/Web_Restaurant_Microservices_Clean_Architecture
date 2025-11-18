using MediatR;
using Inventory.Application.DTOs.Requests.Ingredients;
using Inventory.Application.DTOs.Responses.Ingredients;

namespace Inventory.Application.Modules.Ingredients.Commands.UpdateIngredients
{
    public sealed record UpdateIngredientsCommand(Guid IdIngredients, IngredientsRequest Request) : IRequest<IngredientsResponse>
    {
    }
}
