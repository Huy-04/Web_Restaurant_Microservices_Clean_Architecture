using Inventory.Application.DTOs.Requests.StockItems;
using Inventory.Application.DTOs.Responses.StockItems;
using MediatR;

namespace Inventory.Application.Modules.StockItems.Commands.CreateStockItems
{
    public sealed record CreateStockItemsCommand(CreateStockItemsRequest Request) : IRequest<StockItemsResponse>
    {
    }
}