using Domain.Core.ValueObjects;
using MediatR;

namespace Inventory.Application.Modules.Stock.Commands.DecreaseStockItemQuantity
{
    public sealed record DecreaseStockItemQuantityCommand(
        Guid StockId,
        Guid StockItemId,
        Measurement Delta) : IRequest<bool>
    {
    }
}
