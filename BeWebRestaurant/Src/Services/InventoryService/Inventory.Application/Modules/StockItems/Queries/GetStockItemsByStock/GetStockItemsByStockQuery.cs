using Inventory.Application.DTOs.Responses.StockItems;
using MediatR;

namespace Inventory.Application.Modules.StockItems.Queries.GetStockItemsByStock
{
    public sealed record GetStockItemsByStockQuery(Guid stockId) : IRequest<IEnumerable<StockItemsResponse>>
    {
    }
}