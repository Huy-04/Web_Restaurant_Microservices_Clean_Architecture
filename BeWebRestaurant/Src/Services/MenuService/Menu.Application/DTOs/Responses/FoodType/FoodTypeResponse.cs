namespace Menu.Application.DTOs.Responses.FoodType
{
    public sealed record FoodTypeResponse(
        Guid IdFoodType,
        string FoodTypeName,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt)
    {
    }
}