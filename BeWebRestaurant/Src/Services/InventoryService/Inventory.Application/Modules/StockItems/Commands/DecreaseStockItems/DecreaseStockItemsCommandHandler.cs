using Application.Core.Mapping.MeasurementMapExtension;
using Domain.Core.Enums;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.RuleFactory;
using Domain.Core.RuleException;
using Inventory.Application.DTOs.Responses.StockItems;
using Inventory.Application.IUnitOfWork;
using Inventory.Application.Mapping.StockItemsMapExtension;
using Inventory.Domain.Common.Messages.FieldNames;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.Modules.StockItems.Commands.DecreaseStockItems
{
    public sealed class DecreaseStockItemsCommandHandler : IRequestHandler<DecreaseStockItemsCommand, StockItemsResponse>
    {
        private readonly IInventoryUnitOfWork _uow;
        private readonly ILogger<DecreaseStockItemsCommandHandler> _logger;

        public DecreaseStockItemsCommandHandler(IInventoryUnitOfWork uow, ILogger<DecreaseStockItemsCommandHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<StockItemsResponse> Handle(DecreaseStockItemsCommand command, CancellationToken token)
        {
            await _uow.BeginTransactionAsync(token);
            try
            {
                var stockItems = await _uow.StockItemsRepo.GetByIdAsync(command.IdStockItems, token);
                if (stockItems is null)
                {
                    _logger.LogWarning("Decrease failed: StockItems with Id={Id} not found", command.IdStockItems);
                    throw RuleFactory.SimpleRuleException(
                        ErrorCategory.NotFound,
                        StockItemsField.IdStockItems,
                        ErrorCode.IdNotFound,
                        new Dictionary<string, object>
                        {
                            {ParamField.Value,command.IdStockItems }
                        });
                }

                var stock = await _uow.StockRepo.GetByIdAsync(stockItems.StockId, token);
                var ingredients = await _uow.IngredientsRepo.GetByIdAsync(stockItems.IngredientsId, token);
                var measurement = command.Request.ToMeasurement();
                stockItems.DecreaseMeasurement(measurement);
                await _uow.StockItemsRepo.Update(stockItems);
                await _uow.CommitTransactionAsync(token);

                return stockItems.ToStockItemsResponse(stock!.StockName, ingredients!.IngredientsName);
            }
            catch (BusinessRuleException bex)
            {
                await _uow.RollbackTransactionAsync(token);
                _logger.LogWarning(bex,
                    "BusinessRule Exception occurred while decreasing StockItems with Id={Id}. Request: {@Request}",
                    command.IdStockItems,
                    command.Request
                );
                throw;
            }
            catch (Exception ex)
            {
                await _uow.RollbackTransactionAsync(token);
                _logger.LogError(ex,
                    "Exception occurred while decreasing StockItems with Id={Id}. Request: {@Request}",
                    command.IdStockItems,
                    command.Request
                );
                throw;
            }
        }
    }
}

