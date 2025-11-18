using MediatR;

namespace Menu.Application.Modules.Food.Commands.DeleteFood
{
    public sealed record DeleteFoodCommand(Guid IdFood) : IRequest<bool>
    {
    }
}