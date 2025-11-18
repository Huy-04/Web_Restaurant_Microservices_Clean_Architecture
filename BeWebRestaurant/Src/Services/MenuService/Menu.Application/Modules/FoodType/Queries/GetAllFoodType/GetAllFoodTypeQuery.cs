using MediatR;
using Menu.Application.DTOs.Responses.FoodType;

namespace Menu.Application.Modules.FoodType.Queries.GetAllFoodType
{
    public sealed record GetAllFoodTypeQuery() : IRequest<IEnumerable<FoodTypeResponse>>
    {
    }
}