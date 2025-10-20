using Common.Mapping.MeasurementMapExtension;
using Domain.Core.Enums;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.RuleFactory;
using Domain.Core.RuleException;
using Inventory.Application.DTOs.Responses.StockItems;
using Inventory.Application.Interfaces;
using Inventory.Application.Mapping.StockItemsMapExtension;
using Inventory.Domain.Common.Messages.FieldNames;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.Modules.StockItems.Commands.IncreaseStockItems
{
    public sealed class IncreaseStockItemCommandHanler : IRequestHandler<IncreaseStockItemCommand, StockItemsResponse>
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<IncreaseStockItemCommandHanler> _logger;

        public IncreaseStockItemCommandHanler(IUnitOfWork uow, ILogger<IncreaseStockItemCommandHanler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<StockItemsResponse> Handle(IncreaseStockItemCommand command, CancellationToken token)
        {
            _logger.LogInformation(
                "Handling increase StockItems with Id={Id}",
                command.IdStockItems
            );

            await _uow.BeginTransactionAsync(token);

            try
            {
                var stockItems = await _uow.StockItemsRepo.GetByIdAsync(command.IdStockItems);
                if (stockItems is null)
                {
                    _logger.LogWarning(
                        "Increase failed: StockItems with Id={Id} not found",
                        command.IdStockItems
                    );
                    throw RuleFactory.SimpleRuleException(
                        ErrorCategory.NotFound,
                        StockItemsField.IdStockItems,
                        ErrorCode.IdNotFound,
                        new Dictionary<string, object>
                        {
                            {ParamField.Value,command.IdStockItems }
                        });
                }
                var stock = await _uow.StockRepo.GetByIdAsync(stockItems.StockId);
                var ingredients = await _uow.IngredientsRepo.GetByIdAsync(stockItems.IngredientsId);
                var measurement = command.Request.ToMeasurement();
                stockItems.IncreaseMeasurement(measurement);
                await _uow.StockItemsRepo.UpdateAsync(stockItems);
                await _uow.CommitAsync(token);

                _logger.LogInformation(
                    "Successfully increase StockItems with Id={Id}",
                    command.IdStockItems
                );
                return stockItems.ToStockItemsResponse(stock!.StockName, ingredients!.IngredientsName);
            }
            catch (BusinessRuleException bex)
            {
                await _uow.RollBackAsync(token);
                _logger.LogWarning(bex,
                    "BusinessRule Exception occurred while increasing StockItems with Id={Id}. Request: {@Request}",
                    command.IdStockItems,
                    command.Request
                );
                throw;
            }
            catch (Exception ex)
            {
                await _uow.RollBackAsync(token);
                _logger.LogError(ex,
                    "Exception occurred while increasing StockItems with Id={Id}. Request: {@Request}",
                    command.IdStockItems,
                    command.Request
                );
                throw;
            }
        }
    }
}