using MediatR;
using Menu.Application.DTOs.Requests.FoodType;
using Menu.Application.DTOs.Responses.FoodType;

namespace Menu.Application.Modules.FoodType.Commands.CreateFoodType
{
    public sealed record CreateFoodTypeCommand(FoodTypeRequest Request) : IRequest<FoodTypeResponse>
    {
    }
}