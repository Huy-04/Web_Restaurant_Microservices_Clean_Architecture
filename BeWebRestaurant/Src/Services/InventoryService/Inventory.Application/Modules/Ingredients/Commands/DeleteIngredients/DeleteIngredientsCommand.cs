using MediatR;

namespace Inventory.Application.Modules.Ingredients.Commands.DeleteIngredients
{
    public sealed record DeleteIngredientsCommand(Guid IdIngredients) : IRequest<bool>
    {
    }
}