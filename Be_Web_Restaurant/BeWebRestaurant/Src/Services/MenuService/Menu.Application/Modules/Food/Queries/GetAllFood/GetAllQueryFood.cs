using MediatR;
using Menu.Application.DTOs.Responses.Food;

namespace Menu.Application.Modules.Food.Queries.GetAllFood
{
    public sealed record GetAllQueryFood() : IRequest<IEnumerable<FoodResponse>>
    {
    }
}