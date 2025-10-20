using Inventory.Application.DTOs.Responses.StockItems;
using MediatR;

namespace Inventory.Application.Modules.StockItems.Queries.GetAllStockItems
{
    public sealed record GetAllIStockItemsQuery() : IRequest<IEnumerable<StockItemsResponse>>
    {
    }
}