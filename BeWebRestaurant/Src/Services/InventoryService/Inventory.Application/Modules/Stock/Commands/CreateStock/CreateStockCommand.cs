using MediatR;
using Inventory.Application.DTOs.Requests.Stock;
using Inventory.Application.DTOs.Responses.Stock;

namespace Inventory.Application.Modules.Stock.Commands.CreateStock
{
    public sealed record CreateStockCommand(StockRequest Request) : IRequest<StockResponse>
    {
    }
}
