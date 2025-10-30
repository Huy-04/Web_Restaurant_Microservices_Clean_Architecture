using Inventory.Application.DTOs.Responses.FoodQuantity;
using Inventory.Application.Interface;
using MediatR;

namespace Inventory.Application.Modules.FoodQuantity.Queries.GetFoodQuantityById
{
    public sealed class GetFoodQuantityByIdQueryHandler : IRequestHandler<GetFoodQuantityByIdQuery, FoodQuantityResponse>
    {
        private readonly IUnitOfWork _uow;

        public GetFoodQuantityByIdQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<FoodQuantityResponse> Handle(GetFoodQuantityByIdQuery query, CancellationToken token)
        {
            var foodRecipeList = await _uow.FoodRecipesRepo.GetByFoodAsync(query.FoodId, token);
            if (!foodRecipeList.Any())
            {
                return new FoodQuantityResponse(query.FoodId, 0);
            }

            var foodRecipeMeasurement = foodRecipeList.ToDictionary(x => x.IngredientsId, x => x.Measurement);
            var stockItemsList = await _uow.StockItemsRepo.GetAllAsync(token);
            var stockItemsMeasurements = stockItemsList.ToDictionary(x => x.IngredientsId, x => x.Measurement);

            var result = foodRecipeMeasurement
                .Where(food => stockItemsMeasurements.ContainsKey(food.Key))
                .ToDictionary(
                    food => food.Key,
                    food => stockItemsMeasurements[food.Key] / food.Value
                );

            var minQuantity = result.Values.Min(m => m.Quantity);
            var quantity = (int)Math.Floor(minQuantity);

            return new FoodQuantityResponse(query.FoodId, quantity);
        }
    }
}