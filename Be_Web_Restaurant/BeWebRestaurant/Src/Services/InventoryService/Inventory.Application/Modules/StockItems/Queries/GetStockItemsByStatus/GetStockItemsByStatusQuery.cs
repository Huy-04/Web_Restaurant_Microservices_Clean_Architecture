using Inventory.Application.DTOs.Responses.StockItems;
using Inventory.Domain.ValueObjects.StockItems;
using MediatR;

namespace Inventory.Application.Modules.StockItems.Queries.GetStockItemsByStatus
{
    public sealed record GetStockItemsByStatusQuery(StockItemsStatus StockItemsStatus) : IRequest<IEnumerable<StockItemsResponse>>
    {
    }
}