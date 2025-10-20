using Common.DTOs.Responses.Measurement;

namespace Inventory.Application.DTOs.Responses.FoodRecipe
{
    public sealed record FoodRecipeResponse(
        Guid IdFoodRecipe,
        Guid FoodId,
        Guid IngredientsId,
        string ingredientsName,
        MeasurementResponse Measurement,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt
        )
    {
    }
}