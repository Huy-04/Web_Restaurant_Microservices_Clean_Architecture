using MediatR;

namespace Inventory.Application.Modules.Stock.Commands.DeleteStock
{
    public sealed record DeleteStockCommand(Guid IdStock) : IRequest<bool>
    {
    }
}