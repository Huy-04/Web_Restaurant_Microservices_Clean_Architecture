using MediatR;
using Menu.Application.DTOs.Responses.Food;
using Menu.Domain.Enums;

namespace Menu.Application.Modules.Food.Queries.GetFoodByStatus
{
    public sealed record GetFoodByStatusQuery(FoodStatusEnum Status) : IRequest<IEnumerable<FoodResponse>>
    {
    }
}