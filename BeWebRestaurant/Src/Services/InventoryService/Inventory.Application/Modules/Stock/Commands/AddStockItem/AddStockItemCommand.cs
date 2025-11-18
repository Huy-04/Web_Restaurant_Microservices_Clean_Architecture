using Domain.Core.ValueObjects;
using Inventory.Application.DTOs.Responses.StockItems;
using MediatR;

namespace Inventory.Application.Modules.Stock.Commands.AddStockItem
{
    public sealed record AddStockItemCommand(
        Guid StockId,
        Guid IngredientsId,
        decimal Capacity,
        Domain.Core.Enums.UnitEnum Unit) : IRequest<StockItemsResponse>
    {
    }
}
