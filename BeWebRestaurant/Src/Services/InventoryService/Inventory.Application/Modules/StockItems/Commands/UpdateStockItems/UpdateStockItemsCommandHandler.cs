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

namespace Inventory.Application.Modules.StockItems.Commands.UpdateStockItems
{
    public sealed class UpdateStockItemsCommandHandler : IRequestHandler<UpdateStockItemsCommand, StockItemsResponse>
    {
        private readonly IInventoryUnitOfWork _uow;
        private readonly ILogger<UpdateStockItemsCommandHandler> _logger;

        public UpdateStockItemsCommandHandler(IInventoryUnitOfWork uow, ILogger<UpdateStockItemsCommandHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<StockItemsResponse> Handle(UpdateStockItemsCommand command, CancellationToken token)
        {
            await _uow.BeginTransactionAsync(token);
            try
            {
                var repo = _uow.StockItemsRepo;

                var stockItems = await repo.GetByIdAsync(command.IdStockItems, token);
                if (stockItems is null)
                {
                    _logger.LogWarning("Update failed: StockItems with Id={Id} not found", command.IdStockItems);
                    throw RuleFactory.SimpleRuleException(
                        ErrorCategory.NotFound,
                        StockItemsField.IdStockItems,
                        ErrorCode.IdNotFound,
                        new Dictionary<string, object>
                        {
                            {ParamField.Value,command.IdStockItems }
                        });
                }

                var stock = await _uow.StockRepo.GetByIdAsync(command.Request.StockId, token);
                if (stock is null)
                {
                    _logger.LogWarning("Update failed: Stock with Id={Id} not found", command.Request.StockId);
                    throw RuleFactory.SimpleRuleException(
                        ErrorCategory.NotFound,
                        StockField.IdStock,
                        ErrorCode.IdNotFound,
                        new Dictionary<string, object>
                        {
                            {ParamField.Value,command.Request.StockId }
                        });
                }

                var ingredients = await _uow.IngredientsRepo.GetByIdAsync(command.Request.IngredientsId, token);
                if (ingredients is null)
                {
                    _logger.LogWarning("Update failed: Ingredients with Id={Id} not found", command.Request.IngredientsId);
                    throw RuleFactory.SimpleRuleException(
                        ErrorCategory.NotFound,
                        IngredientsField.IdIngredients,
                        ErrorCode.IdNotFound,
                        new Dictionary<string, object>
                        {
                            {ParamField.Value,command.Request.IngredientsId }
                        });
                }

                if (await _uow.StockItemsRepo.ExistsByStockIdAndIngredientsIdAsync(command.Request.StockId, command.Request.IngredientsId, token, stockItems.Id))
                {
                    _logger.LogWarning("Update failed: StockItems with StockId={StockId} and IngredientsId={IngredientsId} already exists",
                        command.Request.StockId, command.Request.IngredientsId);
                    throw RuleFactory.SimpleRuleException(
                        ErrorCategory.Conflict,
                        StockItemsField.StockIdAndIngredientsId,
                        ErrorCode.DuplicateEntry,
                        new Dictionary<string, object>
                        {
                            { ParamField.Value,
                                new
                                {
                                    StockId = stockItems.StockId,
                                    IngredientsId = stockItems.IngredientsId
                                }
                            }
                        });
                }

                stockItems.ApplyStockItems(command.Request);
                await _uow.StockItemsRepo.Update(stockItems);
                await _uow.CommitTransactionAsync(token);

                return stockItems.ToStockItemsResponse(stock.StockName, ingredients.IngredientsName);
            }
            catch (BusinessRuleException bex)
            {
                await _uow.RollbackTransactionAsync(token);
                _logger.LogWarning(bex,
                    "BusinessRule Exception occurred while updating StockItems with Id={Id}. Request: {@Request}",
                    command.IdStockItems,
                    command.Request
                );
                throw;
            }
            catch (Exception ex)
            {
                await _uow.RollbackTransactionAsync(token);
                _logger.LogError(ex,
                    "Exception occurred while updating StockItems with Id={Id}. Request: {@Request}",
                    command.IdStockItems,
                    command.Request
                );
                throw;
            }
        }
    }
}

