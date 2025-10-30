using MediatR;
using Menu.Application.DTOs.Requests.Food;
using Menu.Application.DTOs.Responses.Food;

namespace Menu.Application.Modules.Food.Commands.UpdateFood
{
    public sealed record UpdateFoodCommand(Guid IdFood, UpdateFoodRequest Request) : IRequest<FoodResponse>
    {
    }
}