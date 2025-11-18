using Domain.Core.Enums;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.RuleFactory;
using Inventory.Application.IUnitOfWork;
using Inventory.Domain.Common.Messages.FieldNames;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.Modules.Stock.Commands.UpdateStockItem
{
    public sealed class UpdateStockItemCapacityCommandHandler : IRequestHandler<UpdateStockItemCapacityCommand, bool>
    {
        private readonly IInventoryUnitOfWork _uow;
        private readonly ILogger<UpdateStockItemCapacityCommandHandler> _logger;

        public UpdateStockItemCapacityCommandHandler(IInventoryUnitOfWork uow, ILogger<UpdateStockItemCapacityCommandHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateStockItemCapacityCommand command, CancellationToken token)
        {
            await _uow.BeginTransactionAsync(token);
            try
            {
                // 1. Validate Stock exists and get with items
                var stock = await _uow.StockRepo.GetByIdAsync(command.StockId, token);
                if (stock is null)
                {
                    _logger.LogWarning("Update stock item capacity failed: Stock with Id={Id} not found", command.StockId);
                    throw RuleFactory.SimpleRuleException(
                        ErrorCategory.NotFound,
                        StockField.IdStock,
                        ErrorCode.IdNotFound,
                        new Dictionary<string, object>
                        {
                            { ParamField.Value, command.StockId }
                        });
                }

                // 2. Update item capacity through aggregate method
                stock.UpdateItemCapacity(command.ItemId, command.NewCapacity);

                // 3. Update Stock (EF Core will cascade to StockItems)
                await _uow.StockRepo.Update(stock);

                // 4. Commit transaction
                await _uow.SaveChangesAsync(token);
                await _uow.CommitTransactionAsync(token);

                _logger.LogInformation("Stock item capacity updated successfully. StockId={StockId}, ItemId={ItemId}", 
                    command.StockId, command.ItemId);

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
