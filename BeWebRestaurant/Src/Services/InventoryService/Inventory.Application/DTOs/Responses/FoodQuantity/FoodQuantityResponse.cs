namespace Inventory.Application.DTOs.Responses.FoodQuantity
{
    public sealed record FoodQuantityResponse
        (Guid idFood,
        int Quantity)
    {
    }
}