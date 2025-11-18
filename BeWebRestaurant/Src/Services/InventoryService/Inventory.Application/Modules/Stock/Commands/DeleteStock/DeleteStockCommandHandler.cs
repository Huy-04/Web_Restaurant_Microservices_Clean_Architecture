using Domain.Core.Enums;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.RuleFactory;
using Domain.Core.RuleException;
using Inventory.Application.IUnitOfWork;
using Inventory.Domain.Common.Messages.FieldNames;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.Modules.Stock.Commands.DeleteStock
{
    public sealed class DeleteStockCommandHandler : IRequestHandler<DeleteStockCommand, bool>
    {
        private readonly IInventoryUnitOfWork _uow;
        private readonly ILogger<DeleteStockCommandHandler> _logger;

        public DeleteStockCommandHandler(IInventoryUnitOfWork uow, ILogger<DeleteStockCommandHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteStockCommand command, CancellationToken token)
        {
            await _uow.BeginTransactionAsync(token);
            try
            {
                var exists = await _uow.StockRepo.ExistsByIdAsync(command.IdStock, token);
                if (!exists)
                {
                    _logger.LogWarning("Delete failed: Stock with Id={Id} not found", command.IdStock);
                    throw RuleFactory.SimpleRuleException(
                        ErrorCategory.NotFound,
                        StockField.IdStock,
                        ErrorCode.IdNotFound,
                        new Dictionary<string, object>
                        {
                            { ParamField.Value, command.IdStock }
                        });
                }

                if (await _uow.StockItemsRepo.ExistsByStockAsync(command.IdStock, token))
                {
                    _logger.LogWarning("Delete failed: Stock with Id={Id} is in use by stock items", command.IdStock);
                    throw RuleFactory.SimpleRuleException(
                        ErrorCategory.Conflict,
                        StockField.IdStock,
                        ErrorCode.InUse,
                        new Dictionary<string, object>
                        {
                            { ParamField.Value, command.IdStock }
                        });
                }

                await _uow.StockRepo.DeleteAsync(command.IdStock, token);
                await _uow.CommitTransactionAsync(token);

                return true;
            }
            catch (BusinessRuleException bex)
            {
                await _uow.RollbackTransactionAsync(token);
                _logger.LogWarning(
                    bex,
                    "BusinessRule Exception occurred while deleting Stock with Id={Id}",
                    command.IdStock
                );
                throw;
            }
            catch (Exception ex)
            {
                await _uow.RollbackTransactionAsync(token);
                _logger.LogError(
                    ex,
                    "Exception occurred while deleting Stock with Id={Id}",
                    command.IdStock
                );
                throw;
            }
        }
    }
}

