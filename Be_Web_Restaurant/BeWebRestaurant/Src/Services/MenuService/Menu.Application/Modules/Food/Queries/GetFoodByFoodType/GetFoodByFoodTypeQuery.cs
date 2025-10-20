using MediatR;
using Menu.Application.DTOs.Responses.Food;

namespace Menu.Application.Modules.Food.Queries.GetFoodByFoodType
{
    public sealed record GetFoodByFoodTypeQuery(Guid FoodTypeId) : IRequest<IEnumerable<FoodResponse>>
    {
    }
}