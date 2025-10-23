using Domain.Core.RuleException;
using Inventory.Application.Interfaces;
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
                var exists = await _uow.FoodRecipesRepo.DeleteAsync(command.IdFoodRecipe);
                if (!exists)
                {
                    await _uow.RollBackAsync(token);
                    _logger.LogWarning("Delete failed: FoodRecipe with Id={Id} not found", command.IdFoodRecipe);
                    return false;
                }

                await _uow.CommitAsync(token);
                return true;
            }
            catch (BusinessRuleException bex)
            {
                await _uow.RollBackAsync(token);
                _logger.LogWarning(
                    bex,
                    "BusinessRule Exception occurred while deleting FoodRecipe with Id={Id}",
                    command.IdFoodRecipe
                );
                throw;
            }
            catch (Exception ex)
            {
                await _uow.RollBackAsync(token);
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