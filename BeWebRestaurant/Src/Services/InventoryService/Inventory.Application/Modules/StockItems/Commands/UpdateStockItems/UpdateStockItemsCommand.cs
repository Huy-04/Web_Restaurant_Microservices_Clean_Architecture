using Inventory.Application.DTOs.Requests.StockItems;
using Inventory.Application.DTOs.Responses.StockItems;
using MediatR;

namespace Inventory.Application.Modules.StockItems.Commands.UpdateStockItems
{
    public sealed record UpdateStockItemsCommand(Guid IdStockItems, UpdateStockItemsRequest Request) : IRequest<StockItemsResponse>
    {
    }
}