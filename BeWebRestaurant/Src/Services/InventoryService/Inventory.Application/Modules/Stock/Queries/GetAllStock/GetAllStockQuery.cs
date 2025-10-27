using Inventory.Application.DTOs.Responses.Stock;
using MediatR;

namespace Inventory.Application.Modules.Stock.Queries.GetAllStock
{
    public sealed record GetAllStockQuery : IRequest<IEnumerable<StockResponse>>
    {
    }
}