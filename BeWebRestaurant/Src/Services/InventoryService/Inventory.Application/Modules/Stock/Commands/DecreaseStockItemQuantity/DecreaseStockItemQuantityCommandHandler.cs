using Domain.Core.Enums;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.RuleFactory;
using Inventory.Application.IUnitOfWork;
using Inventory.Domain.Common.Messages.FieldNames;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.Modules.Stock.Commands.DecreaseStockItemQuantity
{
    public sealed class DecreaseStockItemQuantityCommandHandler : IRequestHandler<DecreaseStockItemQuantityCommand, bool>
    {
        private readonly IInventoryUnitOfWork _uow;
        private readonly ILogger<DecreaseStockItemQuantityCommandHandler> _logger;

        public DecreaseStockItemQuantityCommandHandler(IInventoryUnitOfWork uow, ILogger<DecreaseStockItemQuantityCommandHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<bool> Handle(DecreaseStockItemQuantityCommand command, CancellationToken token)
        {
            await _uow.BeginTransactionAsync(token);
            try
            {
                // 1. Validate Stock exists
                var stock = await _uow.StockRepo.GetByIdAsync(command.StockId, token);
                if (stock is null)
                {
                    _logger.LogWarning("Decrease stock item quantity failed: Stock with Id={Id} not found", command.StockId);
                    throw RuleFactory.SimpleRuleException(
                        ErrorCategory.NotFound,
                        StockField.IdStock,
                        ErrorCode.IdNotFound,
                        new Dictionary<string, object>
                        {
                            { ParamField.Value, command.StockId }
                        });
                }

                // 2. Decrease quantity through aggregate method
                stock.DecreaseItemQuantity(command.StockItemId, command.Delta);

                // 3. Update Stock (EF Core will cascade to StockItems)
                await _uow.StockRepo.Update(stock);

                // 4. Commit transaction
                await _uow.SaveChangesAsync(token);
                await _uow.CommitTransactionAsync(token);

                _logger.LogInformation("Stock item quantity decreased successfully. StockId={StockId}, ItemId={ItemId}", 
                    command.StockId, command.StockItemId);

                return true;
            }
            catch
            {
                await _uow.RollbackTransactionAsync(token);
                throw;
            }
        }
    }
}
