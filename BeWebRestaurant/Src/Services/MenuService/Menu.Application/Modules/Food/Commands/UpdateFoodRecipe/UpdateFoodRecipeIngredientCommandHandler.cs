using Domain.Core.Enums;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.RuleFactory;
using Menu.Application.IUnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Menu.Application.Modules.Food.Commands.UpdateFoodRecipe
{
    public sealed class UpdateFoodRecipeIngredientCommandHandler : IRequestHandler<UpdateFoodRecipeIngredientCommand, bool>
    {
        private readonly IMenuUnitOfWork _uow;
        private readonly ILogger<UpdateFoodRecipeIngredientCommandHandler> _logger;

        public UpdateFoodRecipeIngredientCommandHandler(
            IMenuUnitOfWork uow,
            ILogger<UpdateFoodRecipeIngredientCommandHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateFoodRecipeIngredientCommand command, CancellationToken token)
        {
            await _uow.BeginTransactionAsync(token);
            try
            {
                // 1. Validate Food exists
                var food = await _uow.WFoodRepo.GetByIdAsync(command.FoodId, token);
                if (food is null)
                {
                    _logger.LogWarning("Update food recipe ingredient failed: Food with Id={Id} not found", command.FoodId);
                    throw RuleFactory.SimpleRuleException(
                        ErrorCategory.NotFound,
                        "FoodId",
                        ErrorCode.IdNotFound,
                        new Dictionary<string, object>
                        {
                            { ParamField.Value, command.FoodId }
                        });
                }

                // 2. Check if new ingredient combination already exists in this food
                var recipes = food.FoodRecipes.ToList();
                if (recipes.Any(r => r.Id != command.RecipeId && r.IngredientsId == command.NewIngredientsId))
                {
                    _logger.LogWarning("Update food recipe ingredient failed: Food already has this ingredient. FoodId={FoodId}, IngredientsId={IngredientsId}", 
                        command.FoodId, command.NewIngredientsId);
                    throw RuleFactory.SimpleRuleException(
                        ErrorCategory.Conflict,
                        "IngredientsId",
                        ErrorCode.DuplicateEntry,
                        new Dictionary<string, object>
                        {
                            { ParamField.Value, command.NewIngredientsId }
                        });
                }

                // 3. Update recipe ingredient through aggregate method
                food.UpdateRecipeIngredient(command.RecipeId, command.NewIngredientsId);

                // 4. Update Food (EF Core will cascade to FoodRecipes)
                _uow.WFoodRepo.Update(food);

                // 5. Commit transaction
                await _uow.SaveChangesAsync(token);
                await _uow.CommitTransactionAsync(token);

                _logger.LogInformation("Food recipe ingredient updated successfully. FoodId={FoodId}, RecipeId={RecipeId}, NewIngredientsId={IngredientsId}", 
                    command.FoodId, command.RecipeId, command.NewIngredientsId);

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
