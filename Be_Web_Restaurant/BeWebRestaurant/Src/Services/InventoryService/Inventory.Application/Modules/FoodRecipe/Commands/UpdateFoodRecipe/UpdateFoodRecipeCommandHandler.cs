using Domain.Core.Enums;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.RuleFactory;
using Domain.Core.RuleException;
using Inventory.Application.DTOs.Responses.FoodRecipe;
using Inventory.Application.Interfaces;
using Inventory.Application.Mapping.FoodRecipeMapExtension;
using Inventory.Domain.Common.Messages.FieldNames;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.Modules.FoodRecipe.Commands.UpdateFoodRecipe
{
    public sealed class UpdateFoodRecipeCommandHandler : IRequestHandler<UpdateFoodRecipeCommand, FoodRecipeResponse>
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<UpdateFoodRecipeCommandHandler> _logger;

        public UpdateFoodRecipeCommandHandler(IUnitOfWork uow, ILogger<UpdateFoodRecipeCommandHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<FoodRecipeResponse> Handle(UpdateFoodRecipeCommand command, CancellationToken token)
        {
            _logger.LogInformation("Handling update FoodRecipe with Id={Id}", command.IdFoodRecipe);

            await _uow.BeginTransactionAsync(token);

            try
            {
                var repo = _uow.FoodRecipesRepo;

                var foodRecipe = await repo.GetByIdAsync(command.IdFoodRecipe);
                if (foodRecipe is null)
                {
                    _logger.LogWarning("Update failed: FoodRecipe with Id={Id} not found", command.IdFoodRecipe);
                    throw RuleFactory.SimpleRuleException
                        (ErrorCategory.NotFound,
                        FoodRecipeField.IdFoodRecipe,
                        ErrorCode.IdNotFound,
                        new Dictionary<string, object>
                        {
                            {ParamField.Value,command.IdFoodRecipe }
                        });
                }

                var entity = command.Request.ToFoodRecipe();
                var ingredients = await _uow.IngredientsRepo.GetByIdAsync(entity.IngredientsId);
                if (ingredients is null)
                {
                    _logger.LogWarning("Update failed: Ingredients with Id={Id} not found", entity.IngredientsId);
                    throw RuleFactory.SimpleRuleException
                        (ErrorCategory.NotFound,
                        IngredientsField.IdIngredients,
                        ErrorCode.IdNotFound,
                        new Dictionary<string, object>
                        {
                            {ParamField.Value,entity.IngredientsId }
                        });
                }

                if (await _uow.FoodRecipesRepo.ExistsByFoodIdAndIngredientsIdAsync(entity.FoodId, entity.IngredientsId, token, foodRecipe.Id))
                {
                    _logger.LogWarning("Update failed: FoodRecipe with FoodId={FoodId} and IngredientsId={IngredientsId} already exists",
                        entity.FoodId, entity.IngredientsId);
                    throw RuleFactory.SimpleRuleException
                        (ErrorCategory.Conflict,
                        FoodRecipeField.IdFoodAndIdIngredients,
                        ErrorCode.DuplicateEntry,
                        new Dictionary<string, object>
                        {
                            { ParamField.Value,
                                new
                                {
                                    FoodId = foodRecipe.FoodId,
                                    IngredientsId = foodRecipe.IngredientsId
                                }
                            }
                        });
                }

                foodRecipe.Update(entity.FoodId, entity.IngredientsId, entity.Measurement);
                await repo.UpdateAsync(foodRecipe);
                await _uow.CommitAsync(token);

                _logger.LogInformation(
                    "Successfully updated FoodRecipe with Id={Id}",
                    command.IdFoodRecipe);

                return foodRecipe.ToFoodRecipeResponse(ingredients.IngredientsName);
            }
            catch (BusinessRuleException bex)
            {
                await _uow.RollBackAsync(token);
                _logger.LogWarning(
                    bex,
                    "BusinessRule Exception occurred while updating FoodRecipe with Id={Id}. Request: {@Request}",
                    command.IdFoodRecipe,
                    command.Request
                );
                throw;
            }
            catch (Exception ex)
            {
                await _uow.RollBackAsync(token);
                _logger.LogError(
                    ex,
                    "Exception occurred while updating FoodRecipe with Id={Id}. Request: {@Request}",
                    command.IdFoodRecipe,
                    command.Request
                );
                throw;
            }
        }
    }
}