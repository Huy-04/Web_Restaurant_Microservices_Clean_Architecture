using MediatR;

namespace Inventory.Application.Modules.StockItems.Commands.DeleteStockItems
{
    public sealed record DeleteStockItemsCommand(Guid IdStockItems) : IRequest<bool>
    {
    }
}