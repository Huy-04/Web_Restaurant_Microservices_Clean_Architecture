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

namespace Inventory.Application.Modules.FoodRecipe.Commands.CreateFoodRecipe
{
    public sealed class CreateFoodRecipeCommandHandler : IRequestHandler<CreateFoodRecipeCommand, FoodRecipeResponse>
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<CreateFoodRecipeCommandHandler> _logger;

        public CreateFoodRecipeCommandHandler(IUnitOfWork uow, ILogger<CreateFoodRecipeCommandHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<FoodRecipeResponse> Handle(CreateFoodRecipeCommand command, CancellationToken token)
        {
            await _uow.BeginTransactionAsync(token);

            try
            {
                var foodRecipe = command.Request.ToFoodRecipe();

                var ingredients = await _uow.IngredientsRepo.GetByIdAsync(foodRecipe.IngredientsId, token);
                if (ingredients is null)
                {
                    _logger.LogWarning("Create failed: Ingredients with Id={Id} not found", foodRecipe.IngredientsId);
                    throw RuleFactory.SimpleRuleException
                        (ErrorCategory.NotFound,
                        IngredientsField.IdIngredients,
                        ErrorCode.IdNotFound,
                        new Dictionary<string, object>
                        {
                            {ParamField.Value,foodRecipe.IngredientsId }
                        });
                }

                if (await _uow.FoodRecipesRepo.ExistsByFoodIdAndIngredientsIdAsync(foodRecipe.FoodId, foodRecipe.IngredientsId))
                {
                    _logger.LogWarning("Create failed: FoodRecipe with FoodId={FoodId} and IngredientsId={IngredientsId} already exists",
                        foodRecipe.FoodId, foodRecipe.IngredientsId);
                    throw RuleFactory.SimpleRuleException
                        (ErrorCategory.Conflict,
                        FoodRecipeField.IdFoodAndIdIngredients,
                        ErrorCode.DuplicateEntry,
                        new Dictionary<string, object>
                        {
                                {
                                    ParamField.Value,
                                    new
                                    {
                                        FoodId = foodRecipe.FoodId,
                                        IngredientsId = foodRecipe.IngredientsId
                                    }
                                }
                        });
                }

                await _uow.FoodRecipesRepo.CreateAsync(foodRecipe);
                await _uow.CommitAsync(token);

                return foodRecipe.ToFoodRecipeResponse(ingredients.IngredientsName);
            }
            catch (BusinessRuleException bex)
            {
                await _uow.RollBackAsync(token);
                _logger.LogWarning(bex,
                    "BusinessRule Exception occurred while creating FoodRecipe. Request: {@Request}",
                    command.Request
                );
                throw;
            }
            catch (Exception ex)
            {
                await _uow.RollBackAsync(token);
                _logger.LogError(ex,
                    "Exception occurred while creating Stock. Request: {@Request}",
                    command.Request
                );
                throw;
            }
        }
    }
}