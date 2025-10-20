using Inventory.Application.DTOs.Responses.Stock;
using MediatR;

namespace Inventory.Application.Modules.Stock.Queries.GetStockById
{
    public sealed record GetStockByIdQuery(Guid IdStock) : IRequest<StockResponse>
    {
    }
}