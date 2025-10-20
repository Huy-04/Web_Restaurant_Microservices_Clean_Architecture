using Domain.Core.Enums;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.RuleFactory;
using Inventory.Application.DTOs.Responses.FoodRecipe;
using Inventory.Application.Interfaces;
using Inventory.Application.Mapping.FoodRecipeMapExtension;
using Inventory.Domain.Common.Messages.FieldNames;
using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.Application.Modules.FoodRecipe.Queries.GetFoodRecipeByFoodAndIngredients
{
    public sealed class GetFoodRecipeByFoodAndIngredientsQHandler : IRequestHandler<GetFoodRecipeByFoodAndIngredientsQuery, IEnumerable<FoodRecipeResponse>>
    {
        private readonly IUnitOfWork _uow;

        public GetFoodRecipeByFoodAndIngredientsQHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<FoodRecipeResponse>> Handle(GetFoodRecipeByFoodAndIngredientsQuery query, CancellationToken token)
        {
            var foodRecipeList = await _uow.FoodRecipesRepo.GetByFoodAndIngredients(query.FoodId, query.IngredientsId);

            if (!foodRecipeList.Any())
            {
                throw RuleFactory.SimpleRuleException
                    (ErrorCategory.NotFound,
                    FoodRecipeField.IdFoodAndIdIngredients,
                    ErrorCode.NoMatchingCombination,
                    new Dictionary<string, object>
                    {
                        { ParamField.Value,$"FoodId:{query.FoodId}; IngredientsId:{query.IngredientsId}" }
                    });
            }
            var ingredientsList = await _uow.IngredientsRepo.GetAllAsync();
            var list = from f in foodRecipeList
                       join i in ingredientsList
                       on f.IngredientsId equals i.Id
                       select f.ToFoodRecipeResponse(i.IngredientsName);
            return list;
        }
    }
}