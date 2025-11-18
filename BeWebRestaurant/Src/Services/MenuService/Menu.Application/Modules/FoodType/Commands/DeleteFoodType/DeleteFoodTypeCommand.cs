using MediatR;

namespace Menu.Application.Modules.FoodType.Commands.DeleteFoodType
{
    public sealed record DeleteFoodTypeCommand(Guid IdFoodType) : IRequest<bool>
    { }
}