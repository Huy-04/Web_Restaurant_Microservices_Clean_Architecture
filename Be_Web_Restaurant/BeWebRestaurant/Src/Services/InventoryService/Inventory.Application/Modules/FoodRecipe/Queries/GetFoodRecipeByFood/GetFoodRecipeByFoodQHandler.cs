using Domain.Core.Enums;
using Domain.Core.Messages.FieldNames;
using Domain.Core.Rule.RuleFactory;
using Inventory.Application.DTOs.Responses.FoodRecipe;
using Inventory.Application.Interfaces;
using Inventory.Application.Mapping.FoodRecipeMapExtension;
using Inventory.Domain.Common.Messages.FieldNames;
using MediatR;

namespace Inventory.Application.Modules.FoodRecipe.Queries.GetFoodRecipeByFood
{
    public sealed class GetFoodRecipeByFoodQHandler : IRequestHandler<GetFoodRecipeByFoodQuery, IEnumerable<FoodRecipeResponse>>
    {
        private readonly IUnitOfWork _uow;

        public GetFoodRecipeByFoodQHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<FoodRecipeResponse>> Handle(GetFoodRecipeByFoodQuery query, CancellationToken token)
        {
            var foodRecipeList = await _uow.FoodRecipesRepo.GetByFoodAsync(query.FoodId);
            if (!foodRecipeList.Any())
            {
                throw RuleFactory.SimpleRuleException
                    (ErrorCategory.NotFound,
                    FoodRecipeField.IdFood,
                    ErrorCode.IdNotFound,
                    new Dictionary<string, object>
                    {
                        {ParamField.Value,query.FoodId }
                    });
            }
            var ingredientsList = await _uow.IngredientsRepo.GetAllAsync(token);

            var list = from f in foodRecipeList
                       join i in ingredientsList
                       on f.IngredientsId equals i.Id
                       select f.ToFoodRecipeResponse(i.IngredientsName);
            return list;
        }
    }
}