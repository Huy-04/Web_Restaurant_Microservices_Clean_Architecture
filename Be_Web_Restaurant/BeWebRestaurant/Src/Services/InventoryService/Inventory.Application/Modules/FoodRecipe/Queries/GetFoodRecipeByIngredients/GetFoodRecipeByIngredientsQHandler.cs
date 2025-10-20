using Domain.Core.Enums;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.RuleFactory;
using Inventory.Application.DTOs.Responses.FoodRecipe;
using Inventory.Application.Interfaces;
using Inventory.Application.Mapping.FoodRecipeMapExtension;
using Inventory.Application.Modules.FoodRecipe.Queries.GetByIngredients;
using Inventory.Domain.Common.Messages.FieldNames;
using MediatR;

namespace Inventory.Application.Modules.FoodRecipe.Queries.GetFoodRecipeByIngredients
{
    public sealed class GetFoodRecipeByIngredientsQHandler : IRequestHandler<GetFoodRecipeByIngredientsQuery, IEnumerable<FoodRecipeResponse>>
    {
        private IUnitOfWork _uow;

        public GetFoodRecipeByIngredientsQHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<FoodRecipeResponse>> Handle(GetFoodRecipeByIngredientsQuery query, CancellationToken token)
        {
            var foodRecipeList = await _uow.FoodRecipesRepo.GetByIngredientsAsync(query.IngredientsId);
            if (!foodRecipeList.Any())
            {
                throw RuleFactory.SimpleRuleException
                    (ErrorCategory.NotFound,
                    IngredientsField.IdIngredients,
                    ErrorCode.IdNotFound,
                    new Dictionary<string, object>
                    {
                        {ParamField.Value,query.IngredientsId }
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