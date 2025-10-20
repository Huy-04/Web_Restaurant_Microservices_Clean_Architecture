using Inventory.Application.DTOs.Responses.FoodQuantity;
using MediatR;

namespace Inventory.Application.Modules.FoodQuantity.Queries.GetFoodQuantity
{
    public sealed record GetFoodQuantityQuery : IRequest<IEnumerable<FoodQuantityResponse>>
    {
    }
}