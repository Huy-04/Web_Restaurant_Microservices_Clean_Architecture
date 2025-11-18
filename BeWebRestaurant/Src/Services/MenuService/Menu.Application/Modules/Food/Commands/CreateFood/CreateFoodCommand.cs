using MediatR;
using Menu.Application.DTOs.Requests.Food;
using Menu.Application.DTOs.Responses.Food;

namespace Menu.Application.Modules.Food.Commands.CreateFood
{
    public sealed record CreateFoodCommand(CreateFoodRequest Request) : IRequest<FoodResponse>
    {
    }
}