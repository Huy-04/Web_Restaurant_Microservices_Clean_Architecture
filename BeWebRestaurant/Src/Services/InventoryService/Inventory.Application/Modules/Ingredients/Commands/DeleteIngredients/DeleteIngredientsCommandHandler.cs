using Domain.Core.Enums;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.RuleFactory;
using Domain.Core.RuleException;
using Inventory.Application.IUnitOfWork;
using Inventory.Domain.Common.Messages.FieldNames;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.Modules.Ingredients.Commands.DeleteIngredients
{
    public sealed class DeleteIngredientsCommandHandler : IRequestHandler<DeleteIngredientsCommand, bool>
    {
        private readonly IInventoryUnitOfWork _uow;
        private readonly ILogger<DeleteIngredientsCommandHandler> _logger;

        public DeleteIngredientsCommandHandler(IInventoryUnitOfWork uow, ILogger<DeleteIngredientsCommandHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteIngredientsCommand command, CancellationToken token)
        {
            await _uow.BeginTransactionAsync(token);
            try
            {
                var exists = await _uow.IngredientsRepo.ExistsByIdAsync(command.IdIngredients, token);
                if (!exists)
                {
                    _logger.LogWarning("Delete failed: Ingredients with Id={Id} not found", command.IdIngredients);
                    throw RuleFactory.SimpleRuleException(
                        ErrorCategory.NotFound,
                        IngredientsField.IdIngredients,
                        ErrorCode.IdNotFound,
                        new Dictionary<string, object>
                        {
                            { ParamField.Value, command.IdIngredients }
                        });
                }

                if (await _uow.StockItemsRepo.ExistsByIngredientsAsync(command.IdIngredients, token))
                {
                    _logger.LogWarning("Delete failed: Ingredients with Id={Id} is in use by stock items", command.IdIngredients);
                    throw RuleFactory.SimpleRuleException(
                        ErrorCategory.Conflict,
                        IngredientsField.IdIngredients,
                        ErrorCode.InUse,
                        new Dictionary<string, object>
                        {
                            { ParamField.Value, command.IdIngredients }
                        });
                }

                if (await _uow.FoodRecipesRepo.ExistsByIngredientsIdAsync(command.IdIngredients, token))
                {
                    _logger.LogWarning("Delete failed: Ingredients with Id={Id} is in use by food recipes", command.IdIngredients);
                    throw RuleFactory.SimpleRuleException(
                        ErrorCategory.Conflict,
                        IngredientsField.IdIngredients,
                        ErrorCode.InUse,
                        new Dictionary<string, object>
                        {
                            { ParamField.Value, command.IdIngredients }
                        });
                }

                await _uow.IngredientsRepo.DeleteAsync(command.IdIngredients, token);
                await _uow.CommitTransactionAsync(token);

                return true;
            }
            catch (BusinessRuleException bex)
            {
                await _uow.RollbackTransactionAsync(token);
                _logger.LogWarning(
                    bex,
                    "BusinessRule Exception occurred while deleting Ingredients with Id={Id}",
                    command.IdIngredients
                );
                throw;
            }
            catch (Exception ex)
            {
                await _uow.RollbackTransactionAsync(token);
                _logger.LogError(
                    ex,
                    "Exception occurred while deleting Ingredients with Id={Id}",
                    command.IdIngredients
                );
                throw;
            }
        }
    }
}

