using Domain.Core.Enums;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.RuleFactory;
using Menu.Application.IUnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Menu.Application.Modules.Food.Commands.UpdateFoodRecipe
{
    public sealed class UpdateFoodRecipeCommandHandler : IRequestHandler<UpdateFoodRecipeCommand, bool>
    {
        private readonly IMenuUnitOfWork _uow;
        private readonly ILogger<UpdateFoodRecipeCommandHandler> _logger;

        public UpdateFoodRecipeCommandHandler(IMenuUnitOfWork uow, ILogger<UpdateFoodRecipeCommandHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateFoodRecipeCommand command, CancellationToken token)
        {
            await _uow.BeginTransactionAsync(token);
            try
            {
                // 1. Validate Food exists
                var food = await _uow.WFoodRepo.GetByIdAsync(command.FoodId, token);
                if (food is null)
                {
                    _logger.LogWarning("Update food recipe failed: Food with Id={Id} not found", command.FoodId);
                    throw RuleFactory.SimpleRuleException(
                        ErrorCategory.NotFound,
                        "FoodId",
                        ErrorCode.IdNotFound,
                        new Dictionary<string, object>
                        {
                            { ParamField.Value, command.FoodId }
                        });
                }

                // 2. Update recipe measurement through aggregate method
                food.UpdateRecipeMeasurement(command.RecipeId, command.Measurement);

                // 3. Update Food (EF Core will cascade to FoodRecipes)
                _uow.WFoodRepo.Update(food);

                // 4. Commit transaction
                await _uow.SaveChangesAsync(token);
                await _uow.CommitTransactionAsync(token);

                _logger.LogInformation("Food recipe updated successfully. FoodId={FoodId}, RecipeId={RecipeId}", 
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
