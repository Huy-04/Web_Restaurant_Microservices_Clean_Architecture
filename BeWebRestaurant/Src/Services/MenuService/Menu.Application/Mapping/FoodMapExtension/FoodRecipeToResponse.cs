using Application.Core.Mapping.MeasurementMapExtension;
using Menu.Application.DTOs.Responses.Food;
using Menu.Domain.Entities;

namespace Menu.Application.Mapping.FoodMapExtension
{
    public static class FoodRecipeToResponse
    {
        public static FoodRecipeResponse ToFoodRecipeResponse(this FoodRecipe foodRecipe)
        {
            return new(
                foodRecipe.Id,
                foodRecipe.FoodId,
                foodRecipe.IngredientsId,
                foodRecipe.Measurement.ToMeasurementResponse());
        }
    }
}