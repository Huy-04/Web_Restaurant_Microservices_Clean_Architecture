using MediatR;
using Menu.Application.DTOs.Requests.FoodType;
using Menu.Application.DTOs.Responses.FoodType;

namespace Menu.Application.Modules.FoodType.Commands.UpdateFoodType
{
    public sealed record UpdateFoodTypeCommand(Guid IdFoodType, FoodTypeRequest Request) : IRequest<FoodTypeResponse>
    {
    }
}