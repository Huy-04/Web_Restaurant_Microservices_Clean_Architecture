using Domain.Core.Enums;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.RuleFactory;
using Inventory.Application.DTOs.Responses.FoodRecipe;
using Inventory.Application.Interfaces;
using Inventory.Application.Mapping.FoodRecipeMapExtension;
using Inventory.Domain.Common.Messages.FieldNames;
using MediatR;

namespace Inventory.Application.Modules.FoodRecipe.Queries.GetFoodRecipeById
{
    public sealed class GetFoodRecipeByIdQHandler : IRequestHandler<GetFoodRecipeByIdQuery, FoodRecipeResponse>
    {
        private readonly IUnitOfWork _uow;

        public GetFoodRecipeByIdQHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<FoodRecipeResponse> Handle(GetFoodRecipeByIdQuery query, CancellationToken token)
        {
            var foodRecipe = await _uow.FoodRecipesRepo.GetByIdAsync(query.IdFoodRecipe);
            if (foodRecipe is null)
            {
                throw RuleFactory.SimpleRuleException
                    (ErrorCategory.NotFound,
                    FoodRecipeField.IdFoodRecipe,
                    ErrorCode.IdNotFound,
                    new Dictionary<string, object>
                    {
                        {ParamField.Value,query.IdFoodRecipe }
                    });
            }
            var ingredients = await _uow.IngredientsRepo.GetByIdAsync(foodRecipe.IngredientsId);
            if (ingredients is null)
            {
                throw RuleFactory.SimpleRuleException
                    (ErrorCategory.NotFound,
                    IngredientsField.IdIngredients,
                    ErrorCode.IdNotFound,
                    new Dictionary<string, object>
                    {
                            {ParamField.Value,foodRecipe.IngredientsId }
                    });
            }
            return foodRecipe.ToFoodRecipeResponse(ingredients.IngredientsName);
        }
    }
}