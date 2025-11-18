using Domain.Core.Enums;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.RuleFactory;
using Menu.Application.IUnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Menu.Application.Modules.Food.Commands.RemoveFoodRecipe
{
    public sealed class RemoveFoodRecipeCommandHandler : IRequestHandler<RemoveFoodRecipeCommand, bool>
    {
        private readonly IMenuUnitOfWork _uow;
        private readonly ILogger<RemoveFoodRecipeCommandHandler> _logger;

        public RemoveFoodRecipeCommandHandler(IMenuUnitOfWork uow, ILogger<RemoveFoodRecipeCommandHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<bool> Handle(RemoveFoodRecipeCommand command, CancellationToken token)
        {
            await _uow.BeginTransactionAsync(token);
            try
            {
                // 1. Validate Food exists
                var food = await _uow.WFoodRepo.GetByIdAsync(command.FoodId, token);
                if (food is null)
                {
                    _logger.LogWarning("Remove food recipe failed: Food with Id={Id} not found", command.FoodId);
                    throw RuleFactory.SimpleRuleException(
                        ErrorCategory.NotFound,
                        "FoodId",
                        ErrorCode.IdNotFound,
                        new Dictionary<string, object>
                        {
                            { ParamField.Value, command.FoodId }
                        });
                }

                // 2. Remove recipe through aggregate method
                food.RemoveRecipe(command.RecipeId);

                // 3. Update Food (EF Core will cascade to FoodRecipes)
                _uow.WFoodRepo.Update(food);

                // 4. Commit transaction
                await _uow.SaveChangesAsync(token);
                await _uow.CommitTransactionAsync(token);

                _logger.LogInformation("Food recipe removed successfully. FoodId={FoodId}, RecipeId={RecipeId}", 
                    command.FoodId, command.RecipeId);

                return true;
            }
            catch
            {
                await _uow.RollbackTransactionAsync(token);
                throw;
            }
        }
    }
}
