using MediatR;
using Inventory.Application.DTOs.Requests.Ingredients;
using Inventory.Application.DTOs.Responses.Ingredients;

namespace Inventory.Application.Modules.Ingredients.Commands.CreateIngredients
{
    public sealed record CreateIngredientsCommand(IngredientsRequest Request) : IRequest<IngredientsResponse>
    {
    }
}
