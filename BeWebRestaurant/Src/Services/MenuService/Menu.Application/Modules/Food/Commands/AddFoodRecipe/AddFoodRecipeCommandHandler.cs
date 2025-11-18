using Domain.Core.Enums;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.RuleFactory;
using Menu.Application.IUnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;
using Menu.Application.DTOs.Responses.Food;
using Application.Core.Mapping.MeasurementMapExtension;

namespace Menu.Application.Modules.Food.Commands.AddFoodRecipe
{
    public sealed class AddFoodRecipeCommandHandler : IRequestHandler<AddFoodRecipeCommand, FoodRecipeResponse>
    {
        private readonly IMenuUnitOfWork _uow;
        private readonly ILogger<AddFoodRecipeCommandHandler> _logger;

        public AddFoodRecipeCommandHandler(IMenuUnitOfWork uow, ILogger<AddFoodRecipeCommandHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<FoodRecipeResponse> Handle(AddFoodRecipeCommand command, CancellationToken token)
        {
            await _uow.BeginTransactionAsync(token);
            try
            {
                var food = await _uow.WFoodRepo.GetByIdAsync(command.Request.FoodId, token);
                if (food is null)
                {
                    _logger.LogWarning("Add food recipe failed: Food with Id={Id} not found", command.Request.FoodId);
                    throw RuleFactory.SimpleRuleException(
                        ErrorCategory.NotFound,
                        "FoodId",
                        ErrorCode.IdNotFound,
                        new Dictionary<string, object>
                        {
                            { ParamField.Value, command.Request.FoodId }
                        });
                }

                var existingRecipe = food.FoodRecipes.FirstOrDefault(r => r.IngredientsId == command.Request.IngredientsId);
                if (existingRecipe != null)
                {
                    _logger.LogWarning("Add food recipe failed: Recipe with IngredientsId={Id} already exists", command.Request.IngredientsId);
                    throw RuleFactory.SimpleRuleException(
                        ErrorCategory.Conflict,
                        "IngredientsId",
                        ErrorCode.DuplicateEntry,
                        new Dictionary<string, object>
                        {
                            { ParamField.Value, command.Request.IngredientsId }
                        });
                }

                food.AddRecipe(command.Request.IngredientsId, command.Request.Measurement.ToMeasurement());

                _uow.WFoodRepo.Update(food);

                await _uow.SaveChangesAsync(token);
                await _uow.CommitTransactionAsync(token);

                var newRecipe = food.FoodRecipes.First(r => r.IngredientsId == command.Request.IngredientsId);

                _logger.LogInformation("Food recipe added successfully. FoodId={FoodId}, RecipeId={RecipeId}",
                    command.Request.FoodId, newRecipe.Id);

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