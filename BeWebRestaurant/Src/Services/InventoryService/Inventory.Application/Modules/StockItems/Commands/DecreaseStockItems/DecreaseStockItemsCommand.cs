using Application.Core.DTOs.Requests.Measurement;
using MediatR;
using Inventory.Application.DTOs.Responses.StockItems;

namespace Inventory.Application.Modules.StockItems.Commands.DecreaseStockItems
{
    public sealed record DecreaseStockItemsCommand(Guid IdStockItems, MeasurementRequest Request) : IRequest<StockItemsResponse>
    {
    }
}
