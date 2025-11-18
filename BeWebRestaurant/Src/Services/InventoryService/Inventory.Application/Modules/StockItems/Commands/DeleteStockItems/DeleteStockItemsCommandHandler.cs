using Domain.Core.Enums;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.RuleFactory;
using Domain.Core.RuleException;
using Inventory.Application.IUnitOfWork;
using Inventory.Domain.Common.Messages.FieldNames;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.Modules.StockItems.Commands.DeleteStockItems
{
    public sealed class DeleteStockItemsCommandHandler : IRequestHandler<DeleteStockItemsCommand, bool>
    {
        private readonly IInventoryUnitOfWork _uow;
        private readonly ILogger<DeleteStockItemsCommandHandler> _logger;

        public DeleteStockItemsCommandHandler(IInventoryUnitOfWork uow, ILogger<DeleteStockItemsCommandHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteStockItemsCommand command, CancellationToken token)
        {
            await _uow.BeginTransactionAsync(token);
            try
            {
                var exists = await _uow.StockItemsRepo.ExistsByIdAsync(command.IdStockItems, token);
                if (!exists)
                {
                    _logger.LogWarning("Delete failed: StockItems with Id={Id} not found", command.IdStockItems);
                    throw RuleFactory.SimpleRuleException(
                        ErrorCategory.NotFound,
                        StockItemsField.IdStockItems,
                        ErrorCode.IdNotFound,
                        new Dictionary<string, object>
                        {
                            { ParamField.Value, command.IdStockItems }
                        });
                }

                await _uow.StockItemsRepo.DeleteAsync(command.IdStockItems, token);
                await _uow.CommitTransactionAsync(token);

                return true;
            }
            catch (BusinessRuleException bex)
            {
                await _uow.RollbackTransactionAsync(token);
                _logger.LogWarning(bex,
                    "BusinessRule Exception occurred while deleting StockItems with Id={Id}",
                    command.IdStockItems
                );
                throw;
            }
            catch (Exception ex)
            {
                await _uow.RollbackTransactionAsync(token);
                _logger.LogError(ex,
                    "Exception occurred while deleting StockItems with Id={Id}",
                    command.IdStockItems
                );
                throw;
            }
        }
    }
}

