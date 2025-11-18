using Domain.Core.ValueObjects;
using MediatR;

namespace Inventory.Application.Modules.Stock.Commands.IncreaseStockItemQuantity
{
    public sealed record IncreaseStockItemQuantityCommand(
        Guid StockId,
        Guid StockItemId,
        Measurement Delta) : IRequest<bool>
    {
    }
}
