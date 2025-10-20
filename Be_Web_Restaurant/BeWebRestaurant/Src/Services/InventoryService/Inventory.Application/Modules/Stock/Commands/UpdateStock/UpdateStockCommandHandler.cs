using Domain.Core.Enums;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.RuleFactory;
using Domain.Core.RuleException;
using Inventory.Application.DTOs.Responses.Stock;
using Inventory.Application.Interfaces;
using Inventory.Application.Mapping.StockMapExtension;
using Inventory.Domain.Common.Messages.FieldNames;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.Modules.Stock.Commands.UpdateStock
{
    public sealed class UpdateStockCommandHandler : IRequestHandler<UpdateStockCommand, StockResponse>
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<UpdateStockCommandHandler> _logger;

        public UpdateStockCommandHandler(IUnitOfWork uow, ILogger<UpdateStockCommandHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<StockResponse> Handle(UpdateStockCommand command, CancellationToken token)
        {
            _logger.LogInformation(
                "Handling update Stock with Id={Id}",
                command.IdStock
            );

            await _uow.BeginTransactionAsync(token);
            try
            {
                var repo = _uow.StockRepo;

                var stock = await repo.GetByIdAsync(command.IdStock, token);
                if (stock is null)
                {
                    _logger.LogWarning(
                        "Update failed: Stock with Id={Id} not found",
                        command.IdStock
                    );
                    throw RuleFactory.SimpleRuleException
                        (ErrorCategory.NotFound,
                        StockField.IdStock,
                        ErrorCode.IdNotFound,
                        new Dictionary<string, object>
                        {
                            {ParamField.Value,command.IdStock }
                        });
                }

                var entity = command.Request.ToStock();
                if (await repo.ExistsByNameAsync(entity.StockName, token, stock.Id))
                {
                    _logger.LogWarning(
                        "Update failed: Stock with Name:'{Name}' already exists",
                        entity.StockName.Value
                    );
                    throw RuleFactory.SimpleRuleException
                        (ErrorCategory.Conflict,
                        StockField.StockName,
                        ErrorCode.NameAlreadyExists,
                        new Dictionary<string, object>
                        {
                            {ParamField.Value,entity.StockName.Value }
                        });
                }

                stock.Update(entity.StockName, entity.Description);
                await repo.UpdateAsync(stock, token);
                await _uow.CommitAsync(token);

                _logger.LogInformation(
                    "Successfully updated Stock with Id={Id}",
                    command.IdStock
                );
                return stock.ToStockResponse();
            }
            catch (BusinessRuleException bex)
            {
                await _uow.RollBackAsync(token);
                _logger.LogWarning(
                    bex,
                    "BusinessRule Exception occurred while updating Stock with Id={Id}. Request: {@Request}",
                    command.IdStock,
                    command.Request
                );
                throw;
            }
            catch (Exception ex)
            {
                await _uow.RollBackAsync(token);
                _logger.LogError(
                    ex,
                    "Exception occurred while updating Stock with Id={Id}. Request: {@Request}",
                    command.IdStock,
                    command.Request
                );
                throw;
            }
        }
    }
}