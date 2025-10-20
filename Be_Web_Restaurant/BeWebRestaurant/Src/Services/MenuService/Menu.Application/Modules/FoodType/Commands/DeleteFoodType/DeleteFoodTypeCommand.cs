using MediatR;

namespace Menu.Application.Modules.FoodTypes.Commands.DeleteFoodType
{
    public sealed record DeleteFoodTypeCommand(Guid IdFoodType)
        : IRequest<bool>
    { }
}