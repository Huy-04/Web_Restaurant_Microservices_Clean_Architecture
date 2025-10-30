using Inventory.Application.DTOs.Responses.FoodRecipe;
using Inventory.Application.Interface;
using Inventory.Application.Mapping.FoodRecipeMapExtension;
using MediatR;

namespace Inventory.Application.Modules.FoodRecipe.Queries.GetAllFoodRecipe
{
    public sealed class GetAllFoodRecipeQHandler : IRequestHandler<GetAllFoodRecipeQuery, IEnumerable<FoodRecipeResponse>>
    {
        private readonly IUnitOfWork _uow;

        public GetAllFoodRecipeQHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<FoodRecipeResponse>> Handle(GetAllFoodRecipeQuery query, CancellationToken token)
        {
            var foodRecipeList = await _uow.FoodRecipesRepo.GetAllAsync(token);
            var ingredientsList = await _uow.IngredientsRepo.GetAllAsync(token);

            var list = from f in foodRecipeList
                       join i in ingredientsList
                       on f.IngredientsId equals i.Id
                       select f.ToFoodRecipeResponse(i.IngredientsName);

            return list;
        }
    }
}