using MediatR;
using Menu.Application.DTOs.Responses.Food;

namespace Menu.Application.Modules.Food.Queries.GetFoodById
{
    public sealed record GetFoodByIdQuery(Guid IdFood) : IRequest<FoodResponse>
    {
    }
}