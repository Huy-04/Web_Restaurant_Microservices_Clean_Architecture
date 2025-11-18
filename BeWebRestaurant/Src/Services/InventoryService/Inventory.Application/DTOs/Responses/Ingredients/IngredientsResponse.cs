namespace Inventory.Application.DTOs.Responses.Ingredients
{
    public sealed record IngredientsResponse(
        Guid IdIngredients,
        string IngredientsName,
        string Description,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt)
    {
    }
}