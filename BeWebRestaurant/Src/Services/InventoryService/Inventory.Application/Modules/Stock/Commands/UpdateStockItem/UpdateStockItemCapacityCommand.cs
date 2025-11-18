using Inventory.Domain.ValueObjects.StockItems;
using MediatR;

namespace Inventory.Application.Modules.Stock.Commands.UpdateStockItem
{
    public sealed record UpdateStockItemCapacityCommand(
        Guid StockId,
        Guid ItemId,
        Capacity NewCapacity) : IRequest<bool>
    {
    }
}
