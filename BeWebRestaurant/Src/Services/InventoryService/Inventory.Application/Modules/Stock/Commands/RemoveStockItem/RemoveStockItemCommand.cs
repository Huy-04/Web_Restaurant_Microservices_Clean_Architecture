using MediatR;

namespace Inventory.Application.Modules.Stock.Commands.RemoveStockItem
{
    public sealed record RemoveStockItemCommand(
        Guid StockId,
        Guid StockItemId) : IRequest<bool>
    {
    }
}
