using MediatR;
using Menu.Application.DTOs.Responses.FoodType;

namespace Menu.Application.Modules.FoodType.Queries.GetFoodTypeById
{
    public sealed record GetFoodTypeByIdQuery(Guid IdFoodType) : IRequest<FoodTypeResponse>
    {
    }
}