using MediatR;
using Menu.Application.DTOs.Responses.Food;

namespace Menu.Application.Modules.Food.Queries.GetFoodByStatus
{
    // Accept only primitives at the boundary; map to domain in the handler
    public sealed record GetFoodByStatusQuery(string Status) : IRequest<IEnumerable<FoodResponse>>
    {
    }
}
