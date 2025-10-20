using MediatR;
using Menu.Application.DTOs.Responses.Food;
using Menu.Domain.ValueObjects.Food;

namespace Menu.Application.Modules.Food.Queries.GetFoodByStatus
{
    public sealed record GetFoodByStatusQuery(FoodStatus foodStatus) : IRequest<IEnumerable<FoodResponse>>
    {
    }
}