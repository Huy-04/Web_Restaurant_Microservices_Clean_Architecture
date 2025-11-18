using Application.Core.DTOs.Requests.Measurement;
using MediatR;
using Inventory.Application.DTOs.Responses.StockItems;

namespace Inventory.Application.Modules.StockItems.Commands.IncreaseStockItems
{
    public sealed record IncreaseStockItemCommand(Guid IdStockItems, MeasurementRequest Request) : IRequest<StockItemsResponse>
    {
    }
}
