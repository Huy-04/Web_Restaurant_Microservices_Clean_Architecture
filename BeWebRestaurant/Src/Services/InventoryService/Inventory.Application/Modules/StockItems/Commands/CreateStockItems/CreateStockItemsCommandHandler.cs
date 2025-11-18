using Domain.Core.Enums;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.RuleFactory;
using Domain.Core.RuleException;
using Inventory.Application.DTOs.Responses.StockItems;
using Inventory.Application.IUnitOfWork;
using Inventory.Application.Mapping.StockItemsMapExtension;
using Inventory.Domain.Common.Messages.FieldNames;
using Inventory.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.Modules.StockItems.Commands.CreateStockItems
{
    public sealed class CreateStockItemsCommandHandler : IRequestHandler<CreateStockItemsCommand, StockItemsResponse>
    {
        private readonly IInventoryUnitOfWork _uow;
        private readonly ILogger<CreateStockItemsCommandHandler> _logger;

        public CreateStockItemsCommandHandler(IInventoryUnitOfWork uow, ILogger<CreateStockItemsCommandHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<StockItemsResponse> Handle(CreateStockItemsCommand command, CancellationToken token)
        {
            await _uow.BeginTransactionAsync(token);
            try
            {
                var stockItems = command.Request.ToStockItems();
                var stock = await _uow.StockRepo.GetByIdAsync(stockItems.StockId, token);
                if (stock is null)
                {
                    _logger.LogWarning("Create failed: Stock with Id={Id} not found", stockItems.StockId);
                    throw RuleFactory.SimpleRuleException(
                        ErrorCategory.NotFound,
                        StockField.IdStock,
                        ErrorCode.IdNotFound,
                        new Dictionary<string, object>
                        {
                            {ParamField.Value,stockItems.StockId }
                        });
                }

                var ingredients = await _uow.IngredientsRepo.GetByIdAsync(stockItems.IngredientsId, token);
                if (ingredients is null)
                {
                    _logger.LogWarning("Create failed: Ingredients with Id={Id} not found", stockItems.IngredientsId);
                    throw RuleFactory.SimpleRuleException(
                        ErrorCategory.NotFound,
                        IngredientsField.IdIngredients,
                        ErrorCode.IdNotFound,
                        new Dictionary<string, object>
                        {
                            {ParamField.Value,stockItems.IngredientsId }
                        });
                }

                if (await _uow.StockItemsRepo.ExistsByStockIdAndIngredientsIdAsync(stockItems.StockId, stockItems.IngredientsId, token))
                {
                    _logger.LogWarning("Create failed: StockItems with StockId={StockId} and IngredientsId={IngredientsId} already exists",
                        stockItems.StockId, stockItems.IngredientsId);
                    throw RuleFactory.SimpleRuleException(
                        ErrorCategory.Conflict,
                        StockItemsField.StockIdAndIngredientsId,
                        ErrorCode.DuplicateEntry,
                        new Dictionary<string, object>
                        {
                            {
                                ParamField.Value,
                                new
                                {
                                    StockId = stockItems.StockId,
                                    IngredientsId = stockItems.IngredientsId
                                }
                            }
                        });
                }

                await _uow.StockItemsRepo.CreateAsync(stockItems, token);
                await _uow.CommitTransactionAsync(token);

                return stockItems.ToStockItemsResponse(stock.StockName, ingredients.IngredientsName);
            }
            catch (BusinessRuleException bex)
            {
                await _uow.RollbackTransactionAsync(token);
                _logger.LogWarning(bex,
                    "BusinessRule Exception occurred while creating StockItems. Request: {@Request}",
                    command.Request
                );
                throw;
            }
            catch (Exception ex)
            {
                await _uow.RollbackTransactionAsync(token);
                _logger.LogError(ex,
                    "Exception occurred while creating StockItems. Request: {@Request}",
                    command.Request
                );
                throw;
            }
        }
    }
}

