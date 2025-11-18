using Domain.Core.Enums;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.RuleFactory;
using Domain.Core.RuleException;
using Inventory.Application.DTOs.Responses.Stock;
using Inventory.Application.IUnitOfWork;
using Inventory.Application.Mapping.StockMapExtension;
using Inventory.Domain.Common.Messages.FieldNames;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.Modules.Stock.Commands.CreateStock
{
    public sealed class CreateStockCommandHandler : IRequestHandler<CreateStockCommand, StockResponse>
    {
        private readonly IInventoryUnitOfWork _uow;
        private readonly ILogger<CreateStockCommandHandler> _logger;

        public CreateStockCommandHandler(IInventoryUnitOfWork uow, ILogger<CreateStockCommandHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<StockResponse> Handle(CreateStockCommand command, CancellationToken token)
        {
            await _uow.BeginTransactionAsync(token);
            try
            {
                var stock = command.Request.ToStock();
                if (await _uow.StockRepo.ExistsByNameAsync(stock.StockName, token))
                {
                    _logger.LogWarning("Create failed: Stock with Name '{Name}' already exists", stock.StockName.Value);
                    throw RuleFactory.SimpleRuleException
                        (ErrorCategory.Conflict,
                        StockField.StockName,
                        ErrorCode.NameAlreadyExists,
                        new Dictionary<string, object>
                        {
                                {ParamField.Value,stock.StockName.Value }
                        });
                }

                await _uow.StockRepo.CreateAsync(stock, token);
                await _uow.CommitTransactionAsync(token);

                return stock.ToStockResponse();
            }
            catch (BusinessRuleException bex)
            {
                await _uow.RollbackTransactionAsync(token);
                _logger.LogWarning(bex,
                    "BusinessRule Exception occurred while creating Stock. Request: {@Request}",
                    command.Request
                );
                throw;
            }
            catch (Exception ex)
            {
                await _uow.RollbackTransactionAsync(token);
                _logger.LogError(ex,
                    "Exception occurred while creating Stock. Request: {@Request}",
                    command.Request
                );
                throw;
            }
        }
    }
}