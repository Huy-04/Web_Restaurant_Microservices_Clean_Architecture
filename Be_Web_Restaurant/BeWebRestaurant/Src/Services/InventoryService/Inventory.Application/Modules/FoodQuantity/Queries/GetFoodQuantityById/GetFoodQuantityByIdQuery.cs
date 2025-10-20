using Inventory.Application.DTOs.Responses.FoodQuantity;
using MediatR;

namespace Inventory.Application.Modules.FoodQuantity.Queries.GetFoodQuantityById
{
    public sealed record GetFoodQuantityByIdQuery(Guid FoodId) : IRequest<FoodQuantityResponse>
    {
    }
}