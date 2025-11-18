using Inventory.Application.DTOs.Responses.StockItems;
using MediatR;

namespace Inventory.Application.Modules.StockItems.Queries.GetStockItemsByStatus
{
    // Accept primitive at the boundary; map to VO in handler
    public sealed record GetStockItemsByStatusQuery(string Status) : IRequest<IEnumerable<StockItemsResponse>>
    {
    }
}
