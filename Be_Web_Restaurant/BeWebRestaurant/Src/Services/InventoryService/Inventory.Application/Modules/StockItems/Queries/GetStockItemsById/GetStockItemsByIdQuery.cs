using Inventory.Application.DTOs.Responses.StockItems;
using MediatR;

namespace Inventory.Application.Modules.StockItems.Queries.GetStockItemsById
{
    public sealed record GetStockItemsByIdQuery(Guid IdStockItems) : IRequest<StockItemsResponse>
    {
    }
}