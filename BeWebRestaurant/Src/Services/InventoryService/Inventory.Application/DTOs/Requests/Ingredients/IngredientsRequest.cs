namespace Inventory.Application.DTOs.Requests.Ingredients
{
    public sealed record IngredientsRequest(
        string IngredientsName,
        string Description)
    {
    }
}