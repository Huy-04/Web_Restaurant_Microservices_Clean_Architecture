using Application.Core.DTOs.Requests.Measurement;
using Application.Core.DTOs.Responses.Measurement;

namespace Menu.Application.DTOs.Responses.Food
{
    public sealed record FoodRecipeResponse(
        Guid IdFoodRecipe,
        Guid FoodId,
        Guid IngredientsId,
        MeasurementResponse Measurement)
    {
    }
}