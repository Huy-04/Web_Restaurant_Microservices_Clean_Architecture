using Inventory.Application.DTOs.Responses.FoodQuantity;
using Inventory.Application.Interface;
using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.Application.Modules.FoodQuantity.Queries.GetFoodQuantity
{
    public sealed class GetFoodQuantityQueryHandler : IRequestHandler<GetFoodQuantityQuery, IEnumerable<FoodQuantityResponse>>
    {
        private readonly IUnitOfWork _uow;

        public GetFoodQuantityQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<FoodQuantityResponse>> Handle(GetFoodQuantityQuery query, CancellationToken token)
        {
            var foodRecipeList = await _uow.FoodRecipesRepo.GetAllAsync(token);
            var stockItemsList = await _uow.StockItemsRepo.GetAllAsync(token);

            var foodRecipeseGroup = foodRecipeList.GroupBy(x => x.FoodId);

            var stockItemsMeasurements = stockItemsList.ToDictionary(
                x => x.IngredientsId,
                x => x.Measurement
            );

            var foodRecipeResults = foodRecipeseGroup.Select(food => new
            {
                FoodId = food.Key,
                Recipe = food.Select(r => new
                {
                    r.IngredientsId,
                    Measurement = stockItemsMeasurements[r.IngredientsId] / r.Measurement
                })
            }).ToList();

            var result = foodRecipeResults.Select(food =>
            {
                var minQuantity = food.Recipe.Min(r => r.Measurement.Quantity);
                var quantity = (int)Math.Floor(minQuantity);
                return new FoodQuantityResponse(food.FoodId, quantity);
            });

            return result;
        }
    }
}