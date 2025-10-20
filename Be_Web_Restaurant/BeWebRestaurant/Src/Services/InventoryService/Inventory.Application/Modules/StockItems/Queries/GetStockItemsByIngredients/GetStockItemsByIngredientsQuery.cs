using Inventory.Application.DTOs.Responses.Stock;
using Inventory.Application.DTOs.Responses.StockItems;
using MediatR;

namespace Inventory.Application.Modules.StockItems.Queries.GetStockItemsByIngredients
{
    public sealed record GetStockItemsByIngredientsQuery(Guid IngredientsId) : IRequest<IEnumerable<StockItemsResponse>>
    {
    }
}