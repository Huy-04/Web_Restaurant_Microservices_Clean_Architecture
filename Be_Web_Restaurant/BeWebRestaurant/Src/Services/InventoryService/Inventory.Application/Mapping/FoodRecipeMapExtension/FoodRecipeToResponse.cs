using Common.Mapping.MeasurementMapExtension;
using Inventory.Application.DTOs.Responses.FoodRecipe;
using Inventory.Domain.Entities;

namespace Inventory.Application.Mapping.FoodRecipeMapExtension
{
    public static class FoodRecipeToResponse
    {
        public static FoodRecipeResponse ToFoodRecipeResponse(this FoodRecipe foodRecipe, string ingredientsName)
        {
            return new(
                foodRecipe.Id,
                foodRecipe.FoodId,
                foodRecipe.IngredientsId,
                ingredientsName,
                foodRecipe.Measurement.ToMeasurementResponse(),
                foodRecipe.CreatedAt,
                foodRecipe.UpdatedAt);
        }
    }
}