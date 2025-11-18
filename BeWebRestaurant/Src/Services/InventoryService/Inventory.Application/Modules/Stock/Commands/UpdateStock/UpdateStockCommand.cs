using MediatR;
using Inventory.Application.DTOs.Requests.Stock;
using Inventory.Application.DTOs.Responses.Stock;

namespace Inventory.Application.Modules.Stock.Commands.UpdateStock
{
    public sealed record UpdateStockCommand(Guid IdStock, StockRequest Request) : IRequest<StockResponse>
    {
    }
}
