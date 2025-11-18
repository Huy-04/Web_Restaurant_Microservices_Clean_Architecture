using Domain.Core.Enums;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.RuleFactory;
using Domain.Core.RuleException;
using Domain.Core.ValueObjects;
using Inventory.Application.DTOs.Responses.StockItems;
using Inventory.Application.IUnitOfWork;
using Inventory.Application.Mapping.StockItemsMapExtension;
using Inventory.Domain.Common.Messages.FieldNames;
using Inventory.Domain.Entities;
using Inventory.Domain.ValueObjects.StockItems;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.Modules.Stock.Commands.AddStockItem
{
    /// <summary>
    /// Refactored handler - StockItem là child entity của Stock
    /// </summary>
    public sealed class AddStockItemCommandHandler : IRequestHandler<AddStockItemCommand, StockItemsResponse>
    {
        private readonly IInventoryUnitOfWork _uow;
        private readonly ILogger<AddStockItemCommandHandler> _logger;

        public AddStockItemCommandHandler(IInventoryUnitOfWork uow, ILogger<AddStockItemCommandHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<StockItemsResponse> Handle(AddStockItemCommand command, CancellationToken token)
        {
            await _uow.BeginTransactionAsync(token);
            try
            {
                // 1. Validate Stock exists
                var stock = await _uow.StockRepo.GetByIdAsync(command.StockId, token);
                if (stock is null)
                {
                    _logger.LogWarning("Add stock item failed: Stock with Id={Id} not found", command.StockId);
                    throw RuleFactory.SimpleRuleException(
                        ErrorCategory.NotFound,
                        StockField.IdStock,
                        ErrorCode.IdNotFound,
                        new Dictionary<string, object>
                        {
                            { ParamField.Value, command.StockId }
                        });
                }

                // 2. Validate Ingredients exists
                var ingredients = await _uow.IngredientsRepo.GetByIdAsync(command.IngredientsId, token);
                if (ingredients is null)
                {
                    _logger.LogWarning("Add stock item failed: Ingredients with Id={Id} not found", command.IngredientsId);
                    throw RuleFactory.SimpleRuleException(
                        ErrorCategory.NotFound,
                        "IngredientsId",
                        ErrorCode.IdNotFound,
                        new Dictionary<string, object>
                        {
                            { ParamField.Value, command.IngredientsId }
                        });
                }

                // 3. Check if item already exists in this stock
                var existingItem = stock.StockItems.FirstOrDefault(i => i.IngredientsId == command.IngredientsId);
                if (existingItem != null)
                {
                    _logger.LogWarning("Add stock item failed: Item with IngredientsId={Id} already exists", command.IngredientsId);
                    throw RuleFactory.SimpleRuleException(
                        ErrorCategory.Conflict,
                        "IngredientsId",
                        ErrorCode.DuplicateEntry,
                        new Dictionary<string, object>
                        {
                            { ParamField.Value, command.IngredientsId }
                        });
                }

                // 4. Add item to Stock aggregate
                var capacity = Capacity.Create(command.Capacity);
                stock.AddItem(command.IngredientsId, capacity, command.Unit);

                // 5. Update Stock (EF Core will cascade to StockItems)
                await _uow.StockRepo.Update(stock);

                // 6. Commit transaction
                await _uow.SaveChangesAsync(token);
                await _uow.CommitTransactionAsync(token);

                // 7. Get the newly added item
                var newItem = stock.StockItems.First(i => i.IngredientsId == command.IngredientsId);

                _logger.LogInformation("Stock item added successfully. StockId={StockId}, ItemId={ItemId}", 
                    command.StockId, newItem.Id);

                // 8. Map and return
                var response = new StockItemsResponse(
                    newItem.Id,
                    command.StockId,
                    stock.StockName.Value,
                    newItem.IngredientsId,
                    ingredients.IngredientsName.Value,
                    new global::Application.Core.DTOs.Responses.Measurement.MeasurementResponse(
                        newItem.Measurement.Quantity,
                        newItem.Measurement.Unit
                    ),
                    newItem.Capacity.Value,
                    newItem.StockItemsStatus.Value,
                    stock.CreatedAt,
                    stock.UpdatedAt
                );
                return response;
            }
            catch
            {
                await _uow.RollbackTransactionAsync(token);
                throw;
            }
        }
    }
}
