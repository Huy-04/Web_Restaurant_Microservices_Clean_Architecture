using Application.Core.DTOs.Requests.Measurement;

namespace Menu.Application.DTOs.Requests.Food
{
    public sealed record FoodRecipeRequest(
        Guid FoodId,
        Guid IngredientsId,
        MeasurementRequest Measurement)
    {
    }
}