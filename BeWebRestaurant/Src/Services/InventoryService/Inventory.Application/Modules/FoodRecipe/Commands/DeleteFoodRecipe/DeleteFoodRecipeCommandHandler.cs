using Domain.Core.Enums;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.RuleFactory;
using Domain.Core.RuleException;
using Inventory.Application.Interface;
using Inventory.Domain.Common.Messages.FieldNames;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.Modules.FoodRecipe.Commands.DeleteFoodRecipe
{
    public sealed class DeleteFoodRecipeCommandHandler : IRequestHandler<DeleteFoodRecipeCommand, bool>
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<DeleteFoodRecipeCommandHandler> _logger;

        public DeleteFoodRecipeCommandHandler(IUnitOfWork uow, ILogger<DeleteFoodRecipeCommandHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteFoodRecipeCommand command, CancellationToken token)
        {
            await _uow.BeginTransactionAsync(token);
            try
            {
                var exists = await _uow.FoodRecipesRepo.ExistsByIdAsync(command.IdFoodRecipe, token);
                if (!exists)
                {
                    _logger.LogWarning("Delete failed: FoodRecipe with Id={Id} not found", command.IdFoodRecipe);
                    throw RuleFactory.SimpleRuleException(
                        ErrorCategory.NotFound,
                        FoodRecipeField.IdFoodRecipe,
                        ErrorCode.IdNotFound,
                        new Dictionary<string, object>
                        {
                            { ParamField.Value, command.IdFoodRecipe }
                        });
                }

                await _uow.FoodRecipesRepo.DeleteAsync(command.IdFoodRecipe, token);
                await _uow.SaveChangesAsync(token);
                await _uow.CommitAsync(token);

                return true;
            }
            catch (BusinessRuleException bex)
            {
                await _uow.RollbackAsync(token);
                _logger.LogWarning(
                    bex,
                    "BusinessRule Exception occurred while deleting FoodRecipe with Id={Id}",
                    command.IdFoodRecipe
                );
                throw;
            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync(token);
                _logger.LogError(
                    ex,
                    "Exception occurred while deleting FoodRecipe with Id={Id}",
                    command.IdFoodRecipe
                );
                throw;
            }
        }
    }
}