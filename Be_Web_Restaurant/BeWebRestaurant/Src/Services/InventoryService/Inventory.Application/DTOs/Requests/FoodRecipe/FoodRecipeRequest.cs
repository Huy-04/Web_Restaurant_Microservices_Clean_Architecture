using Application.Core.DTOs.Requests.Measurement;

namespace Inventory.Application.DTOs.Requests.FoodRecipe
{
    public sealed record FoodRecipeRequest(
        Guid FoodId,
        Guid IngredientsId,
        MeasurementRequest Measurement
        )
    {
    }
}